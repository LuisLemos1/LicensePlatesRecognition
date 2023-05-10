using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LeituraMatriculas
{
    class ProcessamentoMatriculas
    {
        private Tesseract ocr;

        public ProcessamentoMatriculas(string dataPath)
        {
            ocr = new Tesseract(dataPath, "por", OcrEngineMode.Default);
        }

        public string ObterMatriculas(string caminhoImagem)
        {
            Mat img = new Mat(caminhoImagem);
            List<String> licenses = new List<String>();
            List<IInputOutputArray> licensePlateImagesList = new List<IInputOutputArray>();
            List<IInputOutputArray> filteredLicensePlateImagesList = new List<IInputOutputArray>();
            List<RotatedRect> detectedLicensePlateRegionList = new List<RotatedRect>();
            string matriculasEncontradas = "";

            using (Mat gray = new Mat())
            using (Mat canny = new Mat())

            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);
                CvInvoke.Canny(gray, canny, 100, 50, 3, false);
                int[,] hierachy = CvInvoke.FindContourTree(canny, contours, ChainApproxMethod.ChainApproxSimple);

                FindLicensePlate(contours, hierachy, 0, gray, canny, licensePlateImagesList, filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
            }

            if (licenses.Count() == 0)
                matriculasEncontradas = "oops, matricula nao encontrada";
            else
            {
                foreach (String l in licenses)
                    matriculasEncontradas += l + "\n";
            }

            return matriculasEncontradas;
        }

        private void FindLicensePlate(
         VectorOfVectorOfPoint contours, int[,] hierachy, int idx, IInputArray gray, IInputArray canny,
         List<IInputOutputArray> licensePlateImagesList, List<IInputOutputArray> filteredLicensePlateImagesList, List<RotatedRect> detectedLicensePlateRegionList,
         List<String> licenses)
        {
            for (; idx >= 0; idx = hierachy[idx, 0])
            {
                int numberOfChildren = GetNumberOfChildren(hierachy, idx);
                // se nao contem caracters, entao nao e uma matricula
                if (numberOfChildren == 0) continue;

                using (VectorOfPoint contour = contours[idx])
                {
                    if (CvInvoke.ContourArea(contour) > 400)
                    {
                        // Se o contour tem menos de 6 children entao nao e uma matricula
                        // Continuar a procurar
                        if (numberOfChildren < 6)
                        {
                            FindLicensePlate(contours, hierachy, hierachy[idx, 2], gray, canny, licensePlateImagesList,
                               filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                            continue;
                        }

                        RotatedRect box = CvInvoke.MinAreaRect(contour);
                        if (box.Angle < -45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle += 90.0f;
                        }
                        else if (box.Angle > 45.0)
                        {
                            float tmp = box.Size.Width;
                            box.Size.Width = box.Size.Height;
                            box.Size.Height = tmp;
                            box.Angle -= 90.0f;
                        }

                        double whRatio = (double)box.Size.Width / box.Size.Height;
                        if (!(3.0 < whRatio && whRatio < 10.0))
                        {
                            if (hierachy[idx, 2] > 0)
                                FindLicensePlate(contours, hierachy, hierachy[idx, 2], gray, canny, licensePlateImagesList,
                                   filteredLicensePlateImagesList, detectedLicensePlateRegionList, licenses);
                            continue;
                        }

                        using (UMat tmp1 = new UMat())
                        using (UMat tmp2 = new UMat())
                        {
                            PointF[] srcCorners = box.GetVertices();

                            PointF[] destCorners = new PointF[] {
                        new PointF(0, box.Size.Height - 1),
                        new PointF(0, 0),
                        new PointF(box.Size.Width - 1, 0),
                        new PointF(box.Size.Width - 1, box.Size.Height - 1)};

                            using (Mat rot = CvInvoke.GetAffineTransform(srcCorners, destCorners))
                            {
                                CvInvoke.WarpAffine(gray, tmp1, rot, Size.Round(box.Size));
                            }

                            Size approxSize = new Size(240, 180);
                            double scale = Math.Min(approxSize.Width / box.Size.Width, approxSize.Height / box.Size.Height);
                            Size newSize = new Size((int)Math.Round(box.Size.Width * scale), (int)Math.Round(box.Size.Height * scale));
                            CvInvoke.Resize(tmp1, tmp2, newSize, 0, 0, Inter.Cubic);

                            //removes some pixels from the edge
                            int edgePixelSize = 2;
                            Rectangle newRoi = new Rectangle(new Point(edgePixelSize, edgePixelSize),
                               tmp2.Size - new Size(2 * edgePixelSize, 2 * edgePixelSize));
                            UMat plate = new UMat(tmp2, newRoi);

                            UMat filteredPlate = FilterPlate(plate);

                            Tesseract.Character[] words;
                            StringBuilder strBuilder = new StringBuilder();
                            string dataPath = Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).FullName).FullName + "\\assets\\tessdata\\";
                            Tesseract ocr = new Tesseract(dataPath, "por", OcrEngineMode.Default);
                            //ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ-1234567890");


                            using (UMat tmp = filteredPlate.Clone())
                            {
                                ocr.SetImage(tmp);
                                ocr.Recognize();
                                words = ocr.GetCharacters();

                                if (words.Length < 5) continue;

                                for (int i = 0; i < words.Length; i++)
                                {
                                    strBuilder.Append(words[i].Text);
                                }
                            }

                            /*
                             * Manipulacao de Strings, pega num substring a partir de regex e transforma a Matricula
                             * no formato XX-XX-XX
                             * 
                             * Regex aceita os seguintes formatos
                             * AA00AA || AA-00-AA || AA 00 AA || AA.00.AA
                             * AA0000
                             * 0000AA
                             * 00AA00
                             */
                            Regex regex = new Regex(@"([A-Z][A-Z](\.?|\-?|\s?)[0-9][0-9](\.?|\-?|\s?)[A-Z][A-Z])|([A-Z][A-Z](\.?|\-?|\s?)[0-9][0-9](\.?|\-?|\s?)[0-9][0-9])|([0-9][0-9](\.?|\-?|\s?)[0-9][0-9](\.?|\-?|\s?)[A-Z][A-Z])|([0-9][0-9](\.?|\-?|\s?)[A-Z][A-Z](\.?|\-?|\s?)[0-9][0-9])");
                            Match match = regex.Match(strBuilder.ToString());

                            if(match.Success)
                            {
                                string[] charRemover = { "-", ".", " " };
                                string matricula = match.Value;

                                // remover characteres entre os digitos e letras
                                foreach (var c in charRemover)
                                    matricula = matricula.Replace(c, string.Empty);

                                // Ira ser apresentado com o seguinte formato XX-XX-XX
                                matricula = matricula.Insert(2, "-").Insert(5, "-");

                                licenses.Add(matricula);
                                licensePlateImagesList.Add(plate);
                                filteredLicensePlateImagesList.Add(filteredPlate);
                                detectedLicensePlateRegionList.Add(box);
                            }
                        }
                    }
                }
            }
        }

        private static UMat FilterPlate(UMat plate)
        {
            UMat thresh = new UMat();
            CvInvoke.Threshold(plate, thresh, 120, 255, ThresholdType.BinaryInv);

            Size plateSize = plate.Size;
            using (Mat plateMask = new Mat(plateSize.Height, plateSize.Width, DepthType.Cv8U, 1))
            using (Mat plateCanny = new Mat())
            using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
            {
                plateMask.SetTo(new MCvScalar(255.0));
                CvInvoke.Canny(plate, plateCanny, 100, 50);
                CvInvoke.FindContours(plateCanny, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);

                int count = contours.Size;
                for (int i = 1; i < count; i++)
                {
                    using (VectorOfPoint contour = contours[i])
                    {

                        Rectangle rect = CvInvoke.BoundingRectangle(contour);
                        if (rect.Height > (plateSize.Height >> 1))
                        {
                            rect.X -= 1; rect.Y -= 1; rect.Width += 2; rect.Height += 2;
                            Rectangle roi = new Rectangle(Point.Empty, plate.Size);
                            rect.Intersect(roi);
                            CvInvoke.Rectangle(plateMask, rect, new MCvScalar(), -1);
                        }
                    }

                }

                thresh.SetTo(new MCvScalar(), plateMask);
            }

            CvInvoke.Erode(thresh, thresh, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);
            CvInvoke.Dilate(thresh, thresh, null, new Point(-1, -1), 1, BorderType.Constant, CvInvoke.MorphologyDefaultBorderValue);

            return thresh;
        }

        private static int GetNumberOfChildren(int[,] hierachy, int idx)
        {
            idx = hierachy[idx, 2];
            if (idx < 0)
                return 0;

            int count = 1;
            while (hierachy[idx, 0] > 0)
            {
                count++;
                idx = hierachy[idx, 0];
            }
            return count;
        }

    }
}
