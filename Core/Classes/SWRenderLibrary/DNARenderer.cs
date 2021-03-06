﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenArt.AST;
using GenArt.Classes;
using GenArt.Core.AST;
using GenArtCoreNative;
using System.Runtime.CompilerServices;

namespace GenArt.Core.Classes.SWRenderLibrary
{
    public class DNARenderer
    {
        private CanvasARGB _drawCanvas = new CanvasARGB(0, 0);

        private SWTriangle _drawTriangle;
        private SWRectangle _drawRectangle;
        private SWElipse _drawElipse;

        private List<DnaPrimitive> [] _primitivesOnRows;
        private byte [] _oneRenderRow;
        private byte[] _oneRenderRow2;

        private CanvasARGB _DestCanvas = null;
      

        public long FillPixels = 0;

        public enum RenderType { WPF, GDI, SoftwareUniversalPolygon, Software, SoftwareByRows, SoftwareByRowsWithFittness, SoftwareByRowsWithFittnessParallel };

        NativeFunctions _nativefunc = new NativeFunctions();



        public CanvasARGB Canvas
        {
            get { return _drawCanvas; }
        }

        public CanvasARGB DestCanvas
        {
            set { _DestCanvas = value; }
        }

        public long Fittness;

        public DNARenderer(int width, int height)
        {
            this._drawTriangle = new SWTriangle();
            this._drawRectangle = new SWRectangle();
            this._drawCanvas = new CanvasARGB(width, height);
          
            this._drawElipse = new SWElipse();
            this._oneRenderRow = new byte[width * 4];
            this._oneRenderRow2 = new byte[width * 4];
            _primitivesOnRows = new List<DnaPrimitive>[height];
            for (int i =0; i < height; i++)
            {
                _primitivesOnRows[i] = new List<DnaPrimitive>();
            }
        }

        private readonly static Color _black = Color.FromArgb(255, 0, 0, 0);
        private readonly static int _blackInt = _black.ToArgb();

        public void RenderDNA(DnaDrawing dna, RenderType typeRender)
        {
            if (typeRender == RenderType.Software) DnaRender_SoftwareTriangle(dna);
            else if (typeRender == RenderType.SoftwareByRows) DnaRender_SoftwareByRows(dna);
            else if (typeRender == RenderType.SoftwareByRowsWithFittness)
                DnaRender_SoftwareByRowsWithFittness(dna);
            else if (typeRender == RenderType.SoftwareByRowsWithFittnessParallel)
                DnaRender_SW_ByRowsWithFittnessParallel(dna);
            //DnaRender_SoftwareByRowsWithFittness_Faster(dna);

        }

        private void DnaRender_SW_ByRowsWithFittnessParallel(DnaDrawing dna)
        {
            short height = this._DestCanvas.HeightPixel;

            int partLenght = height / 2;
            if (partLenght < 1) throw new NotImplementedException();

            Task<long> tLines = Task<long>.Run(() => DnaRender_SoftwareByRowsWithFittnessPartial(dna, this._oneRenderRow, 0, partLenght - 1));

            Task<long> tLines2 = Task<long>.Run(() => DnaRender_SoftwareByRowsWithFittnessPartial(dna, this._oneRenderRow2, partLenght, height - 1));

            Task.WaitAll(tLines, tLines2);

            this.Fittness = tLines.Result + tLines2.Result;
            
        }

        private void DnaRender_SoftwareTriangle(DnaDrawing dna)
        {

            //nativefunc.ClearFieldByColor(this._drawCanvas.Data, dna.BackGround.BrushColor.ToArgb());

            //FillPixels += this._drawCanvas.CountPixels;
            //_drawCanvas.FastClearColor(dna.BackGround.BrushColor);

            DnaPrimitive [] dnaPolygons = dna.Polygons;
            /*DnaPrimitive [] dnaPolygons = dna.Polygons.ToArray();

            for (int i = 1; i < dnaPolygons.Length; i++)
            {
                int index = i;

                DnaPrimitive tmp = dnaPolygons[index];
                   
                while(index>0 && !DnaDrawing.IsPrimitiveInterleaving(dnaPolygons[index],dnaPolygons[index-1]))
                {
                    dnaPolygons[index] = dnaPolygons[index - 1];
                    index--;
                }

                dnaPolygons[index] = tmp; 
            }*/

            _nativefunc.ClearFieldByColor(this._drawCanvas.Data,(int)dna.BackGround.ColorAsUInt);// _blackInt);
            int polyCount = dnaPolygons.Length;
            for (int i = 0; i < polyCount; i++)
            {
                DnaPrimitive polygon = dnaPolygons[i];
                //FillPixels += polygon.GetPixelSizePolygon();


                if (polygon as DnaPolygon != null)
                {
                   // if(DnaPolygon.IsNotSmallAngles(polygon.Points))
                    this._drawTriangle.RenderTriangle(polygon.Points, _drawCanvas, (int)polygon.Brush.ColorAsUInt);
                }
                else if (polygon is DnaTriangleStrip)
                {
                    this._drawTriangle.RenderTriangleStrip(polygon.Points, _drawCanvas, (int)polygon.Brush.ColorAsUInt);
                }
                else if (polygon is DnaRectangle)
                {
                    this._drawRectangle.Render((DnaRectangle)polygon, _drawCanvas);
                }
                else if (polygon is DnaElipse)
                {
                    this._drawElipse.Render((DnaElipse)polygon, _drawCanvas);
                }


            }

            /* this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(100, 100), Height = 4, Width = 3 }, _drawCanvas);
             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(90, 100), Height = 3, Width = 3 }, _drawCanvas);
             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(80, 100), Height = 2, Width = 3 }, _drawCanvas);
             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(70, 100), Height = 1, Width = 3 }, _drawCanvas);
             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(60, 100), Height = 1, Width = 1 }, _drawCanvas);

             this._drawRectangle.Render(new DnaRectangle() { StartPoint = new DnaPoint(10, 150), EndPoint = new DnaPoint(34, 199), Brush = new DnaBrush(255, 255, 0, 0) }, this._drawCanvas);

             this._drawRectangle.Render(new DnaRectangle() { StartPoint = new DnaPoint(36, 150), EndPoint = new DnaPoint(60, 198), Brush = new DnaBrush(255, 255, 0, 0) }, this._drawCanvas);

             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(10, 150), Height = 49, Width = 25, Brush = new DnaBrush(255, 0, 255, 0) }, _drawCanvas);
             this._drawElipse.Render(new DnaElipse() { StartPoint = new DnaPoint(36, 150), Height = 48, Width = 25, Brush = new DnaBrush(255, 0, 255, 0) }, _drawCanvas);
          */
        }

       
       

        int [] listRowsForFill = new int[1000 * 3 + 3];

        private void DnaRender_SoftwareByRows(DnaDrawing dna)
        {
            DnaPrimitive [] dnaPolygons = dna.Polygons;
            int polyCount = dnaPolygons.Length;
            int pixelHigh = this._drawCanvas.HeightPixel;

            //int [] listRowsForFill = new int[dnaPolygons.Length * 3 + 3];
            int listRowsFFIndex = 3;


            int canvasIndexRow = 0;
            int colorBackground = (int)dna.BackGround.ColorAsUInt;

            listRowsForFill[0] = 0;
            listRowsForFill[1] = _drawCanvas.WidthPixel;
            listRowsForFill[2] = colorBackground;

            byte [] tmp = this._drawCanvas.Data;
            for (int y = 0; y < pixelHigh; y++)
            {
                int startRowIndex = canvasIndexRow;

                for (int i = 0; i < polyCount; i++)
                {
                    int startX = 0;
                    int endX = 0;

                    DnaPrimitive primitive = dnaPolygons[i];


                    if (primitive.GetRangeWidthByRow(y, ref startX, ref endX))
                    {
                        listRowsForFill[listRowsFFIndex] = startRowIndex + startX;
                        listRowsForFill[listRowsFFIndex + 1] = endX - startX + 1;
                        listRowsForFill[listRowsFFIndex + 2] = (int)primitive.Brush.ColorAsUInt;
                        listRowsFFIndex += 3;
                    }
                }

                canvasIndexRow += _drawCanvas.WidthPixel;

                //if ((y & 1) == 1)
                {
                    _nativefunc.RenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, tmp);
                    //SafeRenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, this._drawCanvas.Data);
                    listRowsFFIndex = 3;
                    listRowsForFill[0] = canvasIndexRow;
                }








            }

        }

       


        /// <summary>
        /// vykresluje pouze zadane rozmezi y radku
        /// </summary>
        /// <param name="dna"></param>
        private long DnaRender_SoftwareByRowsWithFittnessPartial(DnaDrawing dna,byte [] oneRenderRow, int startY, int endY )
        {
            long fittness = 0;

            DnaPrimitive[] dnaPolygons = dna.Polygons;

            int polyCount = dnaPolygons.Length;
            int pixelHigh = (endY>= this._drawCanvas.HeightPixel)? this._drawCanvas.HeightPixel : endY;
            byte[] destCanvasData = this._DestCanvas.Data;
            //int [] listRowsForFill = new int[dnaPolygons.Length * 3 + 3];
            int listRowsFFIndex = 3;



            int colorBackground = (int)dna.BackGround.ColorAsUInt;

            //listRowsForFill[0] = 0;
            //listRowsForFill[1] = _drawCanvas.WidthPixel;
            //listRowsForFill[2] = colorBackground;

            int rowIndex = _drawCanvas.Width * startY;

            byte[] tmp = oneRenderRow;



            for (int y = startY; y < pixelHigh; y += 1)
            {
                //for(int i = 0;i<100;i++)
                _nativefunc.ClearFieldByColorInt(tmp, 0, _drawCanvas.WidthPixel, colorBackground);
                // _nativefunc.ClearFieldByColorInt(tmp, 0, _drawCanvas.WidthPixel, colorBackground);

                //_nativefunc.NewRowApplyColor64(tmp,
                //        0, 100,
                //        (int)dnaPolygons[0].Brush.ColorAsUInt);


                for (int i = 0; i < polyCount; i++)
                {

                    int startX = 0;
                    int endX = 0;
                    bool result = false;

                    DnaPrimitive primitive = dnaPolygons[i];



                    DnaPolygon dp = primitive as DnaPolygon;
                    if (dp != null)
                        result = dp.GetRangeWidthByRow(y, ref startX, ref endX);
                    else
                    {
                        DnaRectangle dr = primitive as DnaRectangle;

                        if (dr != null) result = dr.GetRangeWidthByRow(y, ref startX, ref endX);
                        else
                            result = primitive.GetRangeWidthByRow(y, ref startX, ref endX);
                    }

                    if (result)
                    {
                        //listRowsForFill[listRowsFFIndex] =  startX;
                        //listRowsForFill[listRowsFFIndex + 1] = endX - startX + 1;
                        //listRowsForFill[listRowsFFIndex + 2] = (int)primitive.Brush.ColorAsUInt;
                        //listRowsFFIndex += 3;

                        if (startX < 0)
                        {
                            //  Console.Write(startX + ",");
                        }
                        else
                        {
                            //ApplyColor(tmp, startX * 4, endX - startX + 1, (int)primitive.Brush.ColorAsUInt);

                            _nativefunc.NewRowApplyColor64(tmp,
                            startX * 4, endX - startX + 1,
                            (int)primitive.Brush.ColorAsUInt);
                        }


                    }
                }

                //Fittness += FitnessCalculator.ComputeFittnessLine_SumSquare(tmp, _DestCanvas.Data, rowIndex);
                fittness += _nativefunc.ComputeFittnessSquareLineSSE_ARGB(tmp, destCanvasData, rowIndex);
                /*
                //if ((y & 1) == 1)
                {
                    _nativefunc.RenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, tmp);
                    //SafeRenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, this._oneRenderRow);
                    listRowsFFIndex = 3;
                    
                }*/

                rowIndex += _drawCanvas.Width * 1;
            }

            return fittness;
        }

        private void DnaRender_SoftwareByRowsWithFittness(DnaDrawing dna)
        {
            Fittness = 0;

            DnaPrimitive [] dnaPolygons = dna.Polygons;

            int polyCount = dnaPolygons.Length;
            int pixelHigh = this._drawCanvas.HeightPixel;
            byte [] destCanvasData =  this._DestCanvas.Data;
            //int [] listRowsForFill = new int[dnaPolygons.Length * 3 + 3];
            int listRowsFFIndex = 3;



            int colorBackground = (int)dna.BackGround.ColorAsUInt;

            //listRowsForFill[0] = 0;
            //listRowsForFill[1] = _drawCanvas.WidthPixel;
            //listRowsForFill[2] = colorBackground;

            int rowIndex = 0;

            byte [] tmp = this._oneRenderRow;

            for (int y = 0; y < pixelHigh; y += 1)
            {
                //for(int i = 0;i<100;i++)
                _nativefunc.ClearFieldByColorInt(tmp, 0, _drawCanvas.WidthPixel, colorBackground);
               // _nativefunc.ClearFieldByColorInt(tmp, 0, _drawCanvas.WidthPixel, colorBackground);

                //_nativefunc.NewRowApplyColor64(tmp,
                //        0, 100,
                //        (int)dnaPolygons[0].Brush.ColorAsUInt);


                 for (int i = 0; i < polyCount; i++)
                 {

                     int startX = 0;
                     int endX = 0;
                     bool result = false;

                     DnaPrimitive primitive = dnaPolygons[i];



                     DnaPolygon dp = primitive as DnaPolygon;
                     if (dp != null)
                         result = dp.GetRangeWidthByRow(y, ref startX, ref endX);
                     else
                     {
                         DnaRectangle dr = primitive as DnaRectangle;

                         if (dr != null) result = dr.GetRangeWidthByRow(y, ref startX, ref endX);
                         else
                             result = primitive.GetRangeWidthByRow(y, ref startX, ref endX);
                     }

                     //if (primitive.GetRangeWidthByRow(y, ref startX, ref endX))
                     if (result)
                     {
                         //listRowsForFill[listRowsFFIndex] =  startX;
                         //listRowsForFill[listRowsFFIndex + 1] = endX - startX + 1;
                         //listRowsForFill[listRowsFFIndex + 2] = (int)primitive.Brush.ColorAsUInt;
                         //listRowsFFIndex += 3;

                         if (startX < 0)
                         {
                           //  Console.Write(startX + ",");
                         }
                         else
                         {
                             //ApplyColor(tmp, startX * 4, endX - startX + 1, (int)primitive.Brush.ColorAsUInt);

                             _nativefunc.NewRowApplyColor64(tmp,
                             startX * 4, endX - startX + 1,
                             (int)primitive.Brush.ColorAsUInt);
                         }


                     }
                 }

                //Fittness += FitnessCalculator.ComputeFittnessLine_SumSquare(tmp, _DestCanvas.Data, rowIndex);
                Fittness += _nativefunc.ComputeFittnessSquareLineSSE_ARGB(tmp, destCanvasData, rowIndex);
                /*
                //if ((y & 1) == 1)
                {
                    _nativefunc.RenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, tmp);
                    //SafeRenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, this._oneRenderRow);
                    listRowsFFIndex = 3;
                    
                }*/

                rowIndex += _drawCanvas.Width * 1;
            }

        }

        private const int CONST_PART_Batch = 2;
        DnaPrimitive[] partPrimitives = new DnaPrimitive[1000];
        int [] computeAlpha = new int[1000];
        private void DnaRender_SoftwareByRowsWithFittness_Faster(DnaDrawing dna)
        {
            Fittness = 0;

            DnaPrimitive [] dnaPolygons = dna.Polygons;

            int polyCount = dnaPolygons.Length;
            int pixelHigh = this._drawCanvas.HeightPixel;
            byte [] destCanvasData =  this._DestCanvas.Data;
            //int [] listRowsForFill = new int[dnaPolygons.Length * 3 + 3];
            int listRowsFFIndex = 3;



            int colorBackground = _black.ToArgb();

            listRowsForFill[0] = 0;
            listRowsForFill[1] = _drawCanvas.WidthPixel;
            listRowsForFill[2] = colorBackground;


            int countPartPrimitives = 0;

            int partRow = 0;

            int rowIndex = 0;
            int partRowCounter = CONST_PART_Batch;

            byte [] tmp = this._oneRenderRow;

            for (int y = 0; y < pixelHigh; y += 1)
            {
                if (partRowCounter == CONST_PART_Batch)
                {
                    countPartPrimitives = 0;
                    int tmpPartRow = partRow + CONST_PART_Batch;
                    for (int i =0; i < dnaPolygons.Length; i++)
                    {
                        int startY = -1,endY = -1;
                        dnaPolygons[i].GetRangeHighSize(ref startY, ref endY);

                        if(!(tmpPartRow < startY || partRow> endY) )
                        //if (!((partRow > startY && partRow > endY) ||
                       //     (tmpPartRow <= startY && tmpPartRow <= endY)))
                        {
                            partPrimitives[countPartPrimitives] = dnaPolygons[i];
                            //computeAlpha[countPartPrimitives] = (((((int)dnaPolygons[i].Brush.ColorAsUInt) >> 24) & 0xff) * 256) / 255;
                            countPartPrimitives++;
                        }
                    }


                    partRowCounter = 1;
                    partRow += CONST_PART_Batch;
                }
                else
                    partRowCounter++;

                //_nativefunc.ClearFieldByColor(tmp, 0, _drawCanvas.WidthPixel, colorBackground);

                for (int i = 0; i < countPartPrimitives; i++)
                {
                    int startX = 0;
                    int endX = 0;

                    DnaPrimitive primitive = partPrimitives[i];


                    if (primitive.GetRangeWidthByRow(y, ref startX, ref endX))
                    {
                        if (startX < 0)
                        {

                        }
                        else
                        {
                            listRowsForFill[listRowsFFIndex] = startX;
                            listRowsFFIndex++;
                            listRowsForFill[listRowsFFIndex] = endX - startX + 1;
                            listRowsFFIndex++;
                            listRowsForFill[listRowsFFIndex] = (int)primitive.Brush.ColorAsUInt;
                            listRowsFFIndex++;
                        }
                        //int alpha256 = computeAlpha[i];// (((((int)primitive.Brush.ColorAsUInt) >> 24) & 0xff) * 256) / 255;

                        //ApplyColor(tmp, startX * 4, endX - startX + 1, (int)primitive.Brush.ColorAsUInt);

                        //_nativefunc.NewRowApplyColor64(tmp, startX * 4, endX - startX + 1,
                        //(int)primitive.Brush.ColorAsUInt,alpha256);
                    }
                }

                //Fittness += FitnessCalculator.ComputeFittnessLine_SumSquare(tmp, _DestCanvas.Data, rowIndex);

                _nativefunc.RenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, tmp);
                //SafeRenderOneRow(listRowsForFill, (listRowsFFIndex / 3) - 1, this._oneRenderRow);
                listRowsFFIndex = 3;

                Fittness += FitnessCalculator.ComputeFittnessLine_SumSquare(tmp, _DestCanvas.Data, rowIndex);



                    //_nativefunc.ComputeFittnessABSLineSSE_ARGB(tmp, destCanvasData, rowIndex);



                rowIndex += _drawCanvas.Width * 1;
            }

        }




        private void DnaRender_SoftwareByRowsSecondVersion(DnaDrawing dna)
        {
            DnaPrimitive [] dnaPolygons = dna.Polygons;
            int polyCount = dnaPolygons.Length;
            int pixelHigh = this._drawCanvas.HeightPixel;

            for (int y = 0; y < pixelHigh; y++)
            {
                this._primitivesOnRows[y].Clear();
            }


            for (int i = 0; i < polyCount; i++)
            {
                DnaPrimitive polygon = dnaPolygons[i];
                if (polygon is DnaRectangle)
                {
                    int startY = 0;
                    int endY = 0;
                    polygon.GetRangeHighSize(ref startY, ref endY);

                    for (int y =startY; y <= endY; y++)
                    {
                        this._primitivesOnRows[y].Add(polygon);
                    }
                }
            }

            int canvasIndexRow = 0;
            int colorBackground = _black.ToArgb();

            for (int y = 0; y < pixelHigh; y++)
            {
                _nativefunc.ClearFieldByColorInt(this._drawCanvas.Data, canvasIndexRow, _drawCanvas.WidthPixel, colorBackground);

                List<DnaPrimitive> primitiveList =this._primitivesOnRows[y];

                int partPolyCount = primitiveList.Count;

                for (int i = 0; i < partPolyCount; i++)
                {
                    DnaPrimitive polygon = primitiveList[i];
                    //FillPixels += polygon.GetPixelSizePolygon();
                    DnaPolygon poly = polygon as DnaPolygon;
                    DnaRectangle rec = polygon as DnaRectangle;
                    DnaElipse elips = polygon as DnaElipse;

                    if (poly != null)
                    {
                        //    this._drawTriangle.RenderTriangle(polygon.Points, _drawCanvas, polygon.Brush.BrushColor);
                    }
                    else if (rec != null)
                    {
                        this._drawRectangle.RenderRow(y, canvasIndexRow, (int)polygon.Brush.ColorAsUInt, rec, _drawCanvas);
                    }
                    else if (elips != null)
                    {
                        //   this._drawElipse.Render((DnaElipse)polygon, _drawCanvas); 
                    }
                }

                canvasIndexRow += _drawCanvas.Width;
            }

        }

        private void DnaRender_SoftwareByRowsFirstVersion(DnaDrawing dna)
        {

            //byte [] colorsAlpha = new byte[dna.Polygons.Length];

            //_drawCanvas.FastClearColor(dna.BackGround.BrushColor);
            DnaPrimitive [] dnaPolygons = dna.Polygons;
            int polyCount = dnaPolygons.Length;

            int canvasIndexRow = 0;
            int pixelHigh = this._drawCanvas.HeightPixel;
            int colorBackground = _black.ToArgb();

            //nativefunc.ClearFieldByColor(this._drawCanvas.Data, colorBackground);
            for (int y = 0; y < pixelHigh; y++)
            {
                _nativefunc.ClearFieldByColorInt(this._drawCanvas.Data, canvasIndexRow, _drawCanvas.WidthPixel, colorBackground);

                for (int i = 0; i < polyCount; i++)
                {
                    DnaPrimitive polygon = dnaPolygons[i];
                    //FillPixels += polygon.GetPixelSizePolygon();
                    DnaPolygon poly = polygon as DnaPolygon;
                    DnaRectangle rec = polygon as DnaRectangle;
                    DnaElipse elips = polygon as DnaElipse;

                    if (poly != null)
                    {
                        //    this._drawTriangle.RenderTriangle(polygon.Points, _drawCanvas, polygon.Brush.BrushColor);
                    }
                    else if (rec != null)
                    {
                        this._drawRectangle.RenderRow(y, canvasIndexRow, (int)polygon.Brush.ColorAsUInt, rec, _drawCanvas);
                    }
                    else if (elips != null)
                    {
                        //   this._drawElipse.Render((DnaElipse)polygon, _drawCanvas); 
                    }


                }

                canvasIndexRow += _drawCanvas.Width;
            }

        }

        private void SafeRenderOneRow(int[] listRowsForApply, int countRows, byte[] canvas)
        {
            _nativefunc.ClearFieldByColorInt(canvas,
                listRowsForApply[0] * 4
                , listRowsForApply[1], listRowsForApply[2]);

            int index = 3;
            for (int rows = 0; rows < countRows; rows++)
            {
                //SWHelpers.RowApplyBlendColorSafeARGB(canvas,
                //    listRowsForApply[index]*4, (listRowsForApply[index] + listRowsForApply[index + 1] - 1)*4,
                //    listRowsForApply[index + 2]);

                _nativefunc.NewRowApplyColor64(canvas,
                    listRowsForApply[index] * 4, listRowsForApply[index + 1],
                    listRowsForApply[index + 2]);

                index += 3;
            }
        }

        private static void ApplyColor(byte[] canvas, int index, int count, int color)
        {
            // convert alpha value from range 0-255 to 0-256
            int alpha256 = ((color) >> 24) & 0xff;
            if (alpha256 == 0xff) alpha256 += 1;
                       

            int invAlpha = 256 - alpha256;

            int b = (color & 0xff) * alpha256;
            int g = ((color >> 8) & 0xff) * alpha256;
            int r = ((color >> 16) & 0xff) * alpha256;

            while (count > 1)
            {
                int tb = canvas[index];
                int tg = canvas[index + 1];
                int tr = canvas[index + 2];
                canvas[index] = (byte)((b + (tb * invAlpha)) >> 8);
                canvas[index + 1] = (byte)((g + (tg * invAlpha)) >> 8);
                canvas[index + 2] = (byte)((r + (tr * invAlpha)) >> 8);

                int tb2 = canvas[index + 4];
                int tg2 = canvas[index + 5];
                int tr2 = canvas[index + 6];
                canvas[index + 4] = (byte)((b + (tb2 * invAlpha)) >> 8);
                canvas[index + 5] = (byte)((g + (tg2 * invAlpha)) >> 8);
                canvas[index + 6] = (byte)((r + (tr2 * invAlpha)) >> 8);

                index += 8;
                count -= 2;
            }

            if (count > 0)
            {
                int tb = canvas[index];
                int tg = canvas[index + 1];
                int tr = canvas[index + 2];

                canvas[index] = (byte)((b + (tb * invAlpha)) >> 8);
                canvas[index + 1] = (byte)((g + (tg * invAlpha)) >> 8);
                canvas[index + 2] = (byte)((r + (tr * invAlpha)) >> 8);
            }
        }

    }
}
