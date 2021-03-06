﻿using System.Collections.Generic;
using System.Xml.Serialization;
using GenArt.Classes;
using System;
using System.Drawing;
using GenArt.Core.Classes;
using GenArt.Core.AST;
using GenArt.Core.Classes.Misc;

namespace GenArt.AST
{
    public class DnaDrawing
    {
        public DnaPrimitive[] Polygons;// { get; set; }
        private short _maxWidth = 0;
        private short _maxHeight = 0;

        public DnaBrush BackGround;

        public bool IsDirty { get; private set; }

        public short Width { get { return _maxWidth; } }
        public short Height { get { return _maxHeight; } }

        #region RecycleElements

        private static ObjectRecyclerTS<DnaPolygon> _RecyclePool_Polygon = new ObjectRecyclerTS<DnaPolygon>();
        private static ObjectRecyclerTS<DnaRectangle> _RecyclePool_Rectangle = new ObjectRecyclerTS<DnaRectangle>();

        #endregion

        public int PointCount
        {
            get
            {
                int pointCount = 0;
                for (int index = 0; index < Polygons.Length; index++)
                    pointCount += Polygons[index].GetCountPoints();

                return pointCount;
            }
        }

        public DnaDrawing(short maxWidth, short maxHeight)
        {
            Polygons = new DnaPrimitive[0];
            BackGround = new DnaBrush(255, 0, 0, 0);
            _maxHeight = maxHeight;
            _maxWidth = maxWidth;
        }

        public void SetDirty()
        {
            IsDirty = true;
        }

        public static void RecyclePrimitive(DnaPrimitive[] primitives)
        {
            Helper_RecyclePrimitiveSave(primitives);
        }

        public static void RecyclePrimitive(DnaPrimitive primitive)
        {
            Helper_RecyclePrimitiveSave(primitive);
        }

        public static void RecycleClear()
        {
            Helper_RecyclePrimitiveClear();
        }


        public void Init()
        {
            Polygons = new DnaPrimitive[Settings.ActivePolygonsMin];
            BackGround = new DnaBrush(255, 0, 0, 0);
            BackGround.InitRandomWithoutAlpha();

            for (int i = 0; i < Settings.ActivePolygonsMin; i++)
                MutationAddPolygon(255, null);

            SetDirty();
        }

        public DnaDrawing Clone()
        {
            var drawing = new DnaDrawing(this._maxWidth, this._maxHeight);
            drawing.Polygons = new DnaPrimitive[Polygons.Length];
            drawing.BackGround = BackGround;

            for (int index = 0; index < Polygons.Length; index++)
            {
                DnaPolygon tmpPoly = Polygons[index] as DnaPolygon;
                if (tmpPoly != null)
                {
                    DnaPolygon recyclePolygon = _RecyclePool_Polygon.GetNewOrRecycle();
                    tmpPoly.Copy(recyclePolygon);
                    drawing.Polygons[index] = recyclePolygon;
                    continue;
                }

                DnaRectangle tmpRect = Polygons[index] as DnaRectangle;
                if (tmpRect != null)
                {
                    DnaRectangle recycleRectangle = _RecyclePool_Rectangle.GetNewOrRecycle();
                    tmpRect.Copy(recycleRectangle);
                    drawing.Polygons[index] = recycleRectangle;
                    continue;
                }

                drawing.Polygons[index] = (DnaPrimitive)Polygons[index].Clone();
            }

            return drawing;
        }


        public void Mutate(CanvasARGB destImage = null)
        {
            if (Tools.WillMutate(Settings.ActiveRemovePolygonMutationRate))
                RemovePolygon(null);

            else if (Tools.WillMutate(Settings.ActiveAddPolygonMutationRate))
                MutationAddPolygon(0, null, destImage);

            else if (Tools.WillMutate(Settings.ActiveMovePolygonMutationRate))
                SwapPolygon();


            {
                for (int index = 0; index < Polygons.Length; index++)
                    Polygons[index].Mutate(0, this, destImage);
            }

        }

        public void MutateBetter(byte mutationRate, ErrorMatrix errorMatrix, CanvasARGB destImage = null, ImageEdges edgePoints = null)
        {
            this.IsDirty = false;

            /// k mutaci pozadi dochazi pouze jednou 
            //if (Tools.GetRandomNumber(0, 10) == 9)
            //{
            //    BackGround.MutateRGBOldWithoutAlpha(this);
            //    SetDirty();
            //}

            do
            {

                //int mutateChange = Tools.GetRandomNumber(0, 1001);



                if (Tools.GetRandomNumber(0, 1001) < 101)
                {
                    //if (Settings.ActivePolygonsMax <= this.Polygons.Length)
                    //    RemovePolygon();
                    if (Settings.ActivePolygonsMax > this.Polygons.Length)
                    {
                        //AddPolygon(mutationRate, errorMatrix, destImage, edgePoints);  

                        //int tmp = Tools.GetRandomNumber(0, 3);
                        //if (tmp == 0)        AddPolygon(mutationRate, errorMatrix, destImage, edgePoints);  
                        //else if (tmp == 1) AddElipse(mutationRate, errorMatrix, destImage, edgePoints);
                        //else AddRectangle(mutationRate, errorMatrix, destImage, edgePoints);

                        //int tmp = Tools.GetRandomNumber(0, 2);
                        //if (tmp == 0) 
                        MutationAddPolygon(mutationRate, errorMatrix, destImage, edgePoints);
                        //else 
                        //    if (tmp == 1) 
                        //else
                        //       MutationAddTriangleStrip(mutationRate, errorMatrix, destImage, edgePoints);
                        // else 
                        //           if (tmp == 2) 

                        //             AddElipse(mutationRate, errorMatrix, null, edgePoints);
                        //else 
                        //    AddRectangle(mutationRate, errorMatrix, destImage, edgePoints);

                        //if (Tools.GetRandomNumber(0, 3) < 1)
                        //    AddPolygon(mutationRate, errorMatrix, destImage, edgePoints);
                        //if (Tools.GetRandomNumber(0, 3) < 1)
                        //{
                        //    AddElipse(mutationRate, errorMatrix, destImage, edgePoints);
                        //}
                        //    if (Tools.GetRandomNumber(0, 3) < 1)
                        //{
                        //    AddRectangle(mutationRate, errorMatrix, destImage, edgePoints);
                        //}

                        // break;
                    }
                }

                if (Tools.GetRandomNumber(0, 1001) < 101)
                {
                    RemovePolygon(errorMatrix);
                    //RemovePolygon(errorMatrix);
                    //break;

                }
                if (Tools.GetRandomNumber(0, 1001) < 51)
                {
                    //SwapPolygon();
                    SwapPolygon3();
                    //break;

                }

                /*if (Tools.GetRandomNumber(0, 1001) < 101)
                {
                    RandomExchangeElipseRectangle();
                    //break;
                }*/



                if (Polygons.Length > 0)
                {
                    //if (Tools.GetRandomNumber(0, 1001) < 51)
                    //{
                    //    Color nearColor = Color.Black;

                    //    int index = Tools.GetRandomNumber(0, Polygons.Length);
                    //    if (Polygons[index] is DnaPolygon)
                    //    {
                    //        nearColor = PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff(Polygons[index].Points, destImage);
                    //    }
                    //    else if (Polygons[index] is DnaRectangle)
                    //    {
                    //        nearColor = PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff((DnaRectangle)Polygons[index], destImage);
                    //    }

                    //    byte alpha = Polygons[index].Brush.Alpha;
                    //    Polygons[index].Brush.SetByColor(nearColor);

                    //    Polygons[index].Brush.Alpha = alpha;
                    //    this.SetDirty();
                    //}


                    if (Tools.GetRandomNumber(0, 1001) < 101)
                    {
                        //int index = GetRNDIndexPolygonBySize(this.Polygons);
                        //int index = GetRNDIndexPolygonByLive(this.Polygons);

                        //int ? tmpIndex = GetRNDPolygonIndex_ByErrorMatrix(errorMatrix);
                        //if (!tmpIndex.HasValue) throw new NotImplementedException("sem se to nesmi dostat.");

                        //int index = tmpIndex.Value;

                        int index = Tools.GetRandomNumber(0, Polygons.Length);

                        //if (Tools.GetRandomNumber(0, 2) == 3)
                        //    Polygons[index].MutateTranspozite(this, destImage);
                        //else
                        //{
                        Polygons[index].Mutate(mutationRate, this, destImage, edgePoints);

                        //Color nearColor = Color.Black;



                        //byte alpha = Polygons[index].Brush.Alpha;
                        //Polygons[index].Brush.SetByColor(nearColor);
                        //Polygons[index].Brush.Alpha = alpha;
                        //}
                    }

                    if (!this.IsDirty ||
                       (this.IsDirty && Tools.GetRandomNumber(0, 1001) < 101))
                    {
                        //int ? tmpIndex = GetRNDPolygonIndex_ByErrorMatrix(errorMatrix);
                        //if (!tmpIndex.HasValue) throw new NotImplementedException("sem se to nesmi dostat.");

                        //int tindex = tmpIndex.Value;
                        int tindex = Tools.GetRandomNumber(0, Polygons.Length);
                        Polygons[tindex].Brush.MutateRGBOld(mutationRate, this);


                        /*List<int> tmpIndex = GetRNDPolygonListIndex_ByErrorMatrix(errorMatrix);
                        for (int i = 0; i < tmpIndex.Count; i++)
                        {
                            Polygons[tmpIndex[i]].Brush.MutateRGBOldnew(mutationRate, this);
                        }*/

                    }
                }

            } while (false);
            //} while (Tools.GetRandomNumber(1, 101) <= 25);
        }

        public void MutateNew(byte mutationRate, CanvasARGB destImage = null, ImageEdges edgePoints = null)
        {
            int remove = 0;
            int add = 0;
            int change = 0;

            for (int i = 0; i < 3; i++)
            {
                //if (Tools.GetRandomNumber(1, 101) > 3) continue;


                if (Tools.GetRandomNumber(0, 100) == 0) remove++;
                else if (Tools.GetRandomNumber(0, 100) == 0) add++;
                else if (Tools.GetRandomNumber(0, 3) == 0)
                    change++;
            }

            if (remove > 0)
            {
                while (remove > 0)
                {
                    RemovePolygon(null);
                    remove--;
                }
            }

            if (add > 0)
            {
                while (add > 0)
                {
                    //MutationAddPolygonNew(mutationRate, destImage, edgePoints);

                    if (Tools.GetRandomNumber(0, 2) < 1)
                    {
                        MutationAddPolygonNew(mutationRate, destImage, edgePoints);
                    }
                    else
                    {
                        MutationAddRectangleNew(mutationRate, destImage, edgePoints);
                    }
                    add--;
                }

            }

            if (Polygons.Length > 0)
            {
                for (int i = 0; i < change; i++)
                {
                    int tindex = Tools.GetRandomNumber(0, Polygons.Length);

                    if (Tools.GetRandomNumber(0, 2) == 0)
                    {
                        
                        Polygons[tindex].Brush.MutateRGBOld(mutationRate, this);
                    }
                    else 
                    {
                        DnaPolygon poly = Polygons[tindex] as DnaPolygon;
                        if (poly != null)
                        {
                            int indexPoint = Tools.GetRandomNumber(0, poly._Points.Length);
                            var tmp = poly._Points[indexPoint];

                            if(edgePoints != null)
                            {
                                DnaPoint endPoint = edgePoints.GetRandomBorderPoint(tmp.X, tmp.Y);
                                //DnaPoint? resultPoint = edgePoints.GetRandomCloserEdgePoint(tmp,10);

                                DnaPoint? resultPoint = edgePoints.GetFirstEdgeOnLineDirection(tmp.X, tmp.Y, endPoint.X, endPoint.Y);

                                if(!resultPoint.HasValue)
                                {
                                    resultPoint = edgePoints.GetRandomEdgePoint();
                                }

                                tmp = resultPoint.Value;
                            }
                            else
                            {
                                Tools.MutatePointByRadial(ref tmp.X, ref tmp.Y, (short)(this._maxWidth - 1), (short)(this._maxHeight - 1), 16);
                            }

                            poly._Points[indexPoint] = tmp;
                        }

                        DnaRectangle rect = Polygons[tindex] as DnaRectangle;
                        if (rect != null)
                        {
                            if (edgePoints != null)
                            {
                                int pp = Tools.GetRandomNumber(0, 2);
                                DnaPoint tmp = (pp == 0) ? rect.StartPoint : rect.EndPoint;
                                
                                DnaPoint endPoint = edgePoints.GetRandomBorderPoint(tmp.X, tmp.Y);
                                //DnaPoint? resultPoint = edgePoints.GetRandomCloserEdgePoint(tmp, 10);

                                DnaPoint? resultPoint = edgePoints.GetFirstEdgeOnLineDirection(tmp.X, tmp.Y, endPoint.X, endPoint.Y);

                                if (!resultPoint.HasValue)
                                {
                                    resultPoint = edgePoints.GetRandomEdgePoint();
                                }

                                if (pp == 0) rect.StartPoint = resultPoint.Value;
                                else rect.EndPoint = resultPoint.Value;
                            }
                            else
                            {
                                if (Tools.GetRandomNumber(0, 2) == 0)
                                {
                                    DnaPoint start = rect.StartPoint;
                                    Tools.MutatePointByRadial(ref start.X, ref start.Y, (short)(this._maxWidth - 1), (short)(this._maxHeight - 1), 16);
                                    rect.StartPoint = start;
                                }
                                else
                                {
                                    DnaPoint end = rect.EndPoint;
                                    Tools.MutatePointByRadial(ref end.X, ref end.Y, (short)(this._maxWidth - 1), (short)(this._maxHeight - 1), 16);
                                    rect.EndPoint = end;
                                }
                            }

                            rect.RepairOrderAxis();
                        }
                    }
                }
            }
        }




        public static bool IsPrimitiveInterleaving(DnaPrimitive who, DnaPrimitive interWith)
        {
            DnaPolygon poly = who as DnaPolygon;
            if (poly != null)
            {
                DnaPoint[] points = poly._Points;
                if (interWith.IsPointInside(points[0]) ||
                   interWith.IsPointInside(points[1]) ||
                   interWith.IsPointInside(points[2]) ||
                   interWith.IsLineCrossed(points[0], points[1]) ||
                   interWith.IsLineCrossed(points[1], points[2]) ||
                   interWith.IsLineCrossed(points[2], points[0]))
                    return true;
                else
                    return false;
            }

            DnaRectangle rec = who as DnaRectangle;
            if (rec != null)
            {
                //DnaPoint [] points = rec.Points;

                DnaPoint tmp = new DnaPoint(rec.EndPoint.X, rec.StartPoint.Y);

                if (interWith.IsLineCrossed(rec.StartPoint, tmp)) return true;
                if (interWith.IsLineCrossed(rec.EndPoint, tmp)) return true;

                tmp.X = rec.StartPoint.X; tmp.Y = rec.EndPoint.Y;
                if (interWith.IsLineCrossed(rec.StartPoint, tmp)) return true;
                if (interWith.IsLineCrossed(rec.EndPoint, tmp)) return true;


                return false;

                //if (interWith.IsLineCrossed(points[0], new DnaPoint(points[1].X, points[0].Y)) ||
                //    interWith.IsLineCrossed(points[0], new DnaPoint(points[0].X, points[1].Y)) ||
                //    interWith.IsLineCrossed(points[1], new DnaPoint(points[1].X, points[0].Y)) ||
                //    interWith.IsLineCrossed(points[1], new DnaPoint(points[0].X, points[1].Y)))

                //    return true;
                //else
                //    return false;
            }

            DnaElipse eli = who as DnaElipse;
            if (eli != null)
            {
                if (interWith is DnaRectangle || interWith is DnaPolygon)
                {
                    return IsPrimitiveInterleaving(interWith, who);
                }

                DnaElipse intEli = who as DnaElipse;
                if (intEli != null)
                {
                    DnaPoint startPoint = eli.StartPoint;
                    DnaPoint endPoint = eli.EndPoint;

                    if (interWith.IsPointInside(startPoint) ||
                    interWith.IsPointInside(new DnaPoint(startPoint.X, endPoint.Y)) ||
                    interWith.IsPointInside(new DnaPoint(endPoint.X, startPoint.Y)) ||
                    interWith.IsPointInside(endPoint))

                        return true;
                    else
                        return false;
                }
            }

            return true;

        }

        public bool SwapPolygon3()
        {

            if (Polygons.Length < 2)
                return false;

            List<DnaPrimitive> tmpPoly = new List<DnaPrimitive>(this.Polygons);
            List<DnaPrimitive> destPoly = new List<DnaPrimitive>();

            while (tmpPoly.Count > 0)
            {
                int index = Tools.GetRandomNumber(0, tmpPoly.Count);
                destPoly.Add(tmpPoly[index]);
                tmpPoly.RemoveAt(index);
            }

            for (int i = 0; i < destPoly.Count; i++)
            {
                this.Polygons[i] = destPoly[i];
            }

            SetDirty();

            return true;
        }


        static int c = 0;
        public bool SwapPolygon2()
        {

            if (Polygons.Length < 2)
                return false;

            //int index = Tools.GetRandomNumber(0, Polygons.Length - 1);
            //int index2 = Tools.GetRandomNumber(0, Polygons.Length - 1);

            //DnaPolygon poly = Polygons[index];
            //Polygons[index] = Polygons[index2];
            //Polygons[index] = poly;

            int index = Tools.GetRandomNumber(0, Polygons.Length);
            bool swapUp = Tools.GetRandomNumber(0, 2) < 1;

            if (swapUp && index + 1 >= Polygons.Length) swapUp = false;
            else if (!swapUp && index == 0) swapUp = true;

            DnaPrimitive poly = Polygons[index];

            if (swapUp)
            {
                int tmpIndex = index - 1;
                while (tmpIndex >= 0 && !IsPrimitiveInterleaving(Polygons[tmpIndex], Polygons[index]))
                    tmpIndex--;

                if (tmpIndex < 0)
                {
                    tmpIndex = index + 1;
                    while (tmpIndex < Polygons.Length && !IsPrimitiveInterleaving(Polygons[tmpIndex], Polygons[index]))
                        tmpIndex++;

                    // nema smysl prohazovat dva polygony nikde se neprekryvaji
                    if (tmpIndex >= Polygons.Length)
                    {
                        c++;
                        return false;
                    }
                }

                Polygons[index] = Polygons[tmpIndex];
                Polygons[tmpIndex] = poly;
            }
            else
            {
                int tmpIndex = index + 1;
                while (tmpIndex < Polygons.Length && !IsPrimitiveInterleaving(Polygons[tmpIndex], Polygons[index]))
                    tmpIndex++;

                if (tmpIndex >= Polygons.Length)
                {
                    tmpIndex = index - 1;
                    while (tmpIndex >= 0 && !IsPrimitiveInterleaving(Polygons[tmpIndex], Polygons[index]))
                        tmpIndex--;

                    // nema smysl prohazovat dva polygony nikde se neprekryvaji
                    if (tmpIndex < 0)
                    {
                        c++;
                        return false;
                    }
                }


                Polygons[index] = Polygons[tmpIndex];
                Polygons[tmpIndex] = poly;

            }




            {

                SetDirty();
            }

            return true;
        }

        public void SwapPolygon()
        {
            if (Polygons.Length < 2)
                return;

            //int index = Tools.GetRandomNumber(0, Polygons.Length - 1);
            //int index2 = Tools.GetRandomNumber(0, Polygons.Length - 1);

            //DnaPolygon poly = Polygons[index];
            //Polygons[index] = Polygons[index2];
            //Polygons[index] = poly;

            int index = Tools.GetRandomNumber(0, Polygons.Length);
            bool swapUp = Tools.GetRandomNumber(0, 2) < 1;

            if (swapUp && index + 1 >= Polygons.Length) swapUp = false;
            else if (!swapUp && index == 0) swapUp = true;

            DnaPrimitive poly = Polygons[index];

            if (swapUp)
            {
                Polygons[index] = Polygons[index + 1];
                Polygons[index + 1] = poly;
            }
            else
            {
                Polygons[index] = Polygons[index - 1];
                Polygons[index - 1] = poly;

            }



            //int indexfrom = Tools.GetRandomNumber(0, Polygons.Length-1);
            //int indexTo = Tools.GetRandomNumber(0, Polygons.Length - 1);
            //if (indexTo != indexfrom)
            //{
            //    if (indexfrom > indexTo)
            //    {
            //        indexTo++;
            //        DnaPolygon tmpPolygon = Polygons[indexfrom];

            //        for (int i = indexfrom; i > indexTo; i--)
            //            Polygons[i] = Polygons[i - 1];

            //        Polygons[indexTo] = tmpPolygon;
            //    }
            //    else if (indexfrom < indexTo)
            //    {
            //        indexTo++;
            //        DnaPolygon tmpPolygon = Polygons[indexfrom];

            //        for (int i = indexfrom; i < indexTo; i++)
            //            Polygons[i] = Polygons[i + 1];

            //        Polygons[indexTo] = tmpPolygon;
            //    }
            {

                SetDirty();
            }
        }

        private void RandomExchangeElipseRectangle()
        {
            if (Polygons.Length == 0)
                return;

            int index = Tools.GetRandomNumber(0, Polygons.Length);

            DnaPrimitive primitive = null;
            int primitiveIndex = 0;

            // find primitive for change
            int tmp = index;
            // down
            while (tmp >= 0)
            {
                if (Polygons[tmp] is DnaElipse || Polygons[tmp] is DnaRectangle)
                {
                    primitive = Polygons[tmp];
                    primitiveIndex = tmp;
                    break;
                }

                tmp--;
            }

            if (primitive == null)
            {
                index++;
                while (index < Polygons.Length)
                {
                    if (Polygons[index] is DnaElipse || Polygons[index] is DnaRectangle)
                    {
                        primitive = Polygons[index];
                        primitiveIndex = index;
                        break;
                    }

                    index++;
                }
            }

            if (primitive != null)
            {
                DnaElipse elipse = primitive as DnaElipse;
                if (elipse != null)
                {
                    DnaRectangle rec = new DnaRectangle();
                    rec.Brush = elipse.Brush;
                    rec.StartPoint = elipse.StartPoint;
                    rec.EndPoint = elipse.EndPoint;

                    Polygons[primitiveIndex] = rec;

                    SetDirty();
                    return;
                }

                DnaRectangle rectangle = primitive as DnaRectangle;

                if (rectangle != null)
                {
                    if (rectangle.Width > 4 && rectangle.Height > 4)
                    {
                        DnaElipse tmpelipse = new DnaElipse();
                        tmpelipse.Brush = rectangle.Brush;
                        tmpelipse.StartPoint = rectangle.StartPoint;
                        tmpelipse.Width = rectangle.Width;
                        tmpelipse.Height = rectangle.Height;


                        Polygons[primitiveIndex] = tmpelipse;

                        SetDirty();
                        return;
                    }
                }
            }

        }

        public void RemovePolygon(ErrorMatrix errorMatrix)
        {
            if (Polygons.Length > Settings.ActivePolygonsMin)
            {
                //int index = GetRNDIndexPolygonBySize(this.Polygons);
                //int index = GetRNDIndexPolygonByLive(this.Polygons);

                int index = 0;
                if (errorMatrix != null)
                {
                    int? tmpIndex = GetRNDPolygonIndex_ByErrorMatrix(errorMatrix);

                    if (!tmpIndex.HasValue) return;

                    index = tmpIndex.Value;
                }
                else
                {
                    index = Tools.GetRandomNumber(0, Polygons.Length);
                }

                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length - 1];

                //if (index > 0)
                //    Array.Copy(Polygons, polygons, index);
                for (int i = 0; i < index; i++)
                    polygons[i] = Polygons[i];

                for (int i = index; i < polygons.Length; i++)
                    polygons[i] = Polygons[i + 1];

                Polygons = polygons;
                SetDirty();
            }
        }



        public void AddDnaPrimitive(DnaPrimitive primitive)
        {
            DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
            Array.Copy(Polygons, polygons, Polygons.Length);

            polygons[polygons.Length - 1] = primitive;
            Polygons = polygons;
        }


        public void MutationAddPolygonNew(byte mutationRate, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                DnaPolygon newPolygon = _RecyclePool_Polygon.GetNewOrRecycle();
                if (edgePoints != null)
                {
                    DnaPolygonBO.InitNewTriangleByEdge(newPolygon, mutationRate,edgePoints, _rawDestImage);
                }
                else
                {
                    DnaPolygonBO.InitNewTriangle(newPolygon, mutationRate, _rawDestImage);
                }

                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newPolygon;
                Polygons = polygons;

                SetDirty();
            }

        }

        public void MutationAddRectangleNew(byte mutationRate, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                DnaRectangle newRectangle = _RecyclePool_Rectangle.GetNewOrRecycle();

                if(edgePoints != null)
                {
                    DnaPolygonBO.InitNewRectangleByEdge(newRectangle, mutationRate, edgePoints, _rawDestImage);
                }
                else
                {
                    DnaPolygonBO.InitNewRectangle(newRectangle, mutationRate, _rawDestImage);
                }
                

                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newRectangle;
                Polygons = polygons;

                SetDirty();
            }

        }

        public void MutationAddPolygon(byte mutationRate, ErrorMatrix errorMatrix, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                //var newPolygon = new DnaPolygon();
                DnaPolygon newPolygon = _RecyclePool_Polygon.GetNewOrRecycle();

                //newPolygon.Init(null);

                if (_rawDestImage != null)
                {
                    bool randomEdge = Tools.GetRandomNumber(0, 1000) < 500;

                    if (randomEdge)
                    {
                        newPolygon.InitByEdge(edgePoints, randomEdge);
                        newPolygon.Brush.InitRandom();
                        newPolygon.Brush.Alpha = 20;
                    }
                    else
                    {
                        newPolygon.InitByEdge(edgePoints, randomEdge);
                        //Color nearColor = GetColorByPolygonPoints(newPolygon.Points, _rawDestImage, width);
                        Color nearColor =
                            //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                            //PolygonColorPredict.GetColorBy_PointsColor_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                            //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                            PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff(newPolygon._Points, _rawDestImage);

                        newPolygon.Brush.SetByColor(nearColor);
                        newPolygon.Brush.Alpha = 20; //(byte)Tools.GetRandomNumber(0, 256);
                                                     //newPolygon.Brush.InitRandom();
                    }
                }
                else
                {
                    newPolygon.Init(mutationRate, errorMatrix, edgePoints);

                    newPolygon.Brush.InitRandom();
                }


                //List<DnaPrimitive> polygons = new List<DnaPrimitive>(Polygons);
                //if (polygons.Count == 0)
                //    polygons.Add(newPolygon);
                //else
                //{
                //    int index = Tools.GetRandomNumber(0, Polygons.Length); 
                //    polygons.Insert(index, newPolygon);
                //}
                //this.Polygons = polygons.ToArray();

                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newPolygon;
                Polygons = polygons;
                //SortOnePolygonByAlpa(polygons.Length - 1);

                SetDirty();
            }
        }

        public void MutationAddTriangleStrip(byte mutationRate, ErrorMatrix errorMatrix, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                var newTriangleStrip = new DnaTriangleStrip();
                newTriangleStrip.Init(mutationRate, errorMatrix, edgePoints);
                //newPolygon.Init(null);

                if (_rawDestImage != null)
                {
                    //Color nearColor = GetColorByPolygonPoints(newPolygon.Points, _rawDestImage, width);
                    Color nearColor =
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff(newTriangleStrip._Points, _rawDestImage);

                    newTriangleStrip.Brush.SetByColor(nearColor);
                    newTriangleStrip.Brush.Alpha = (byte)Tools.GetRandomNumber(0, 256);
                    //newPolygon.Brush.InitRandom();
                }
                else
                {
                    newTriangleStrip.Brush.InitRandom();
                }


                //List<DnaPrimitive> polygons = new List<DnaPrimitive>(Polygons);
                //if (polygons.Count == 0)
                //    polygons.Add(newPolygon);
                //else
                //{
                //    int index = Tools.GetRandomNumber(0, Polygons.Length); 
                //    polygons.Insert(index, newPolygon);
                //}
                //this.Polygons = polygons.ToArray();

                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newTriangleStrip;
                Polygons = polygons;
                //SortOnePolygonByAlpa(polygons.Length - 1);

                SetDirty();
            }
        }

        public void AddRectangle(byte mutationRate, ErrorMatrix errorMatrix, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                var newRectangle = _RecyclePool_Rectangle.GetNewOrRecycle();
                newRectangle.Init(mutationRate, errorMatrix, edgePoints);


                if (_rawDestImage != null)
                {
                    //Color nearColor = GetColorByPolygonPoints(newPolygon.Points, _rawDestImage, width);
                    Color nearColor =
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff(newRectangle, _rawDestImage);

                    newRectangle.Brush.SetByColor(nearColor);
                    //newPolygon.Brush.InitRandom();
                }
                else
                {
                    newRectangle.Brush.InitRandom();
                }


                //List<DnaPrimitive> polygons = new List<DnaPrimitive>(Polygons);
                //if (polygons.Count == 0)
                //    polygons.Add(newRectangle);
                //else
                //{
                //    int index = Tools.GetRandomNumber(0, Polygons.Length);
                //    polygons.Insert(index, newRectangle);
                //}
                //this.Polygons = polygons.ToArray();


                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newRectangle;
                Polygons = polygons;

                SetDirty();
            }
        }

        public void AddElipse(byte mutationRate, ErrorMatrix errorMatrix, CanvasARGB _rawDestImage = null, ImageEdges edgePoints = null)
        {
            if (Polygons.Length < Settings.ActivePolygonsMax)
            {
                var newElipse = new DnaElipse();
                newElipse.Init(mutationRate, errorMatrix, edgePoints);


                if (_rawDestImage != null)
                {
                    //Color nearColor = GetColorByPolygonPoints(newPolygon.Points, _rawDestImage, width);
                    Color nearColor =
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        //PolygonColorPredict.GetColorBy_PointsColor_MiddleEdgePoints_MiddlePoint_AlphaDiff(newPolygon.Points, _rawDestImage);
                        PolygonColorPredict.GetColorBy_PC_MEP_MEOPAM_MP_AlphaDiff(newElipse, _rawDestImage);

                    newElipse.Brush.SetByColor(nearColor);
                    //newPolygon.Brush.InitRandom();
                }
                else
                {
                    newElipse.Brush.InitRandom();
                }


                //List<DnaPrimitive> polygons = new List<DnaPrimitive>(Polygons);
                //if (polygons.Count == 0)
                //    polygons.Add(newElipse);
                //else
                //{
                //    int index = Tools.GetRandomNumber(0, Polygons.Length);
                //    polygons.Insert(index, newElipse);
                //}
                //this.Polygons = polygons.ToArray();


                DnaPrimitive[] polygons = new DnaPrimitive[Polygons.Length + 1];
                Array.Copy(Polygons, polygons, Polygons.Length);

                polygons[polygons.Length - 1] = newElipse;
                Polygons = polygons;

                SetDirty();
            }
        }

        private int? GetRNDPolygonIndex_ByErrorMatrix(ErrorMatrix errorMatrix)
        {
            List<int> polygons = GetRNDPolygonListIndex_ByErrorMatrix(errorMatrix);

            if (polygons.Count == 0) return null;

            int polygonIndex = Tools.GetRandomNumber(0, polygons.Count);
            return polygons[polygonIndex];
        }


        private List<int> GetRNDPolygonListIndex_ByErrorMatrix(ErrorMatrix errorMatrix)
        {
            List<int> result = new List<int>(2);

            if (this.Polygons.Length == 1)
            {
                result.Add(0);
                return result;
            }
            else if (this.Polygons.Length > 1)
            {
                int counter = 0;
                do
                {
                    if (counter >= 10)
                    {
                        result.Add(0);
                        break;
                    }

                    int matrixIndex = errorMatrix.GetRNDMatrixRouleteIndex();
                    Rectangle tileArea = errorMatrix.GetTileByErrorMatrixIndex(matrixIndex);
                    DnaRectangle rec = new DnaRectangle((short)tileArea.X, (short)tileArea.Y, (short)tileArea.Width, (short)tileArea.Height);

                    for (int index = 0; index < this.Polygons.Length; index++)
                    {
                        DnaPrimitive polygon = this.Polygons[index];
                        if (IsPrimitiveInterleaving(rec, polygon)) result.Add(index);

                        //if ( IsPointInRectangle(tileArea, polygon.Points[0])) polygonsId.Add(index);
                        //else if (IsPointInRectangle(tileArea, polygon.Points[1])) polygonsId.Add(index);
                        //else if (IsPointInRectangle(tileArea, polygon.Points[2])) polygonsId.Add(index);


                    }

                    counter++;
                } while (result.Count == 0);
            }

            // if no pygons in dna   
            return result;
        }

        private static bool IsPointInRectangle(Rectangle area, DnaPoint point)
        {
            int diffX = area.X - point.X;
            int diffY = area.Y - point.Y;

            return diffX >= 0 && diffX < area.Width &&
               diffY >= 0 && diffY < area.Height;

        }

        #region Helpers

        private static void Helper_RecyclePrimitiveSave(DnaPrimitive[] primitives)
        {
            for (int i = 0; i < primitives.Length; i++)
            {
                Helper_RecyclePrimitiveSave(primitives[i]);
            }
        }

        private static void Helper_RecyclePrimitiveSave(DnaPrimitive primitive)
        {
            DnaPolygon poly = primitive as DnaPolygon;
            if (poly != null)
                _RecyclePool_Polygon.PutForRecycle(poly);
            DnaRectangle rect = primitive as DnaRectangle;
            if (rect != null)
                _RecyclePool_Rectangle.PutForRecycle(rect);

        }

        private static void Helper_RecyclePrimitiveClear()
        {

            _RecyclePool_Polygon.Clear();
            _RecyclePool_Rectangle.Clear();
        }


        #endregion


    }


}