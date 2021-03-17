using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Dreambuild.AutoCAD.Internal;
using System;
using System.Collections.Generic;
using Dreambuild.AutoCAD;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using System.IO;

namespace AutoCADCommands
{
    public static class MyTool
    {
        /// <summary>
        /// 以下代码展示了如何设置当前用户坐标系为世界坐标系。
        /// </summary>
        public static void SetUCS2WCS()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            doc.Editor.CurrentUserCoordinateSystem = Matrix3d.Identity;
            doc.Editor.Regen();
        }

        /// <summary>
        /// Create a new UCS, make it active, and translate the coordinates of a point into the UCS coordinates
        /// https://knowledge.autodesk.com/search-result/caas/CloudHelp/cloudhelp/2017/ENU/AutoCAD-NET/files/GUID-096085E3-5AD5-4454-BF10-C9177FDB5979-htm.html
        /// </summary>
        public static void NewUCS()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the UCS table for read
                UcsTable acUCSTbl;
                acUCSTbl = acTrans.GetObject(acCurDb.UcsTableId,
                    OpenMode.ForRead) as UcsTable;

                UcsTableRecord acUCSTblRec;

                // Check to see if the "New_UCS" UCS table record exists
                if (acUCSTbl.Has("New_UCS") == false)
                {
                    acUCSTblRec = new UcsTableRecord();
                    acUCSTblRec.Name = "New_UCS";

                    // Open the UCSTable for write
                    acUCSTbl.UpgradeOpen();

                    // Add the new UCS table record
                    acUCSTbl.Add(acUCSTblRec);
                    acTrans.AddNewlyCreatedDBObject(acUCSTblRec, true);

                    acUCSTblRec.Dispose();
                }
                else
                {
                    acUCSTblRec = acTrans.GetObject(acUCSTbl["New_UCS"],
                        OpenMode.ForWrite) as UcsTableRecord;
                }

                acUCSTblRec.Origin = new Point3d(4, 5, 3);
                acUCSTblRec.XAxis = new Vector3d(1, 0, 0);
                acUCSTblRec.YAxis = new Vector3d(0, 1, 0);

                // Open the active viewport
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                    OpenMode.ForWrite) as ViewportTableRecord;

                // Display the UCS Icon at the origin of the current viewport
                acVportTblRec.IconAtOrigin = true;
                acVportTblRec.IconEnabled = true;

                // Set the UCS current
                acVportTblRec.SetUcs(acUCSTblRec.ObjectId);
                acDoc.Editor.UpdateTiledViewportsFromDatabase();

                // Display the name of the current UCS
                UcsTableRecord acUCSTblRecActive;
                acUCSTblRecActive = acTrans.GetObject(acVportTblRec.UcsName,
                    OpenMode.ForRead) as UcsTableRecord;

                Application.ShowAlertDialog("The current UCS is: " +
                                            acUCSTblRecActive.Name);

                PromptPointResult pPtRes;
                PromptPointOptions pPtOpts = new PromptPointOptions("");

                // Prompt for a point
                pPtOpts.Message = "\nEnter a point: ";
                pPtRes = acDoc.Editor.GetPoint(pPtOpts);

                Point3d pPt3dWCS;
                Point3d pPt3dUCS;

                // If a point was entered, then translate it to the current UCS
                if (pPtRes.Status == PromptStatus.OK)
                {
                    pPt3dWCS = pPtRes.Value;
                    pPt3dUCS = pPtRes.Value;

                    // Translate the point from the current UCS to the WCS
                    Matrix3d newMatrix = new Matrix3d();
                    newMatrix = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                        Vector3d.XAxis,
                        Vector3d.YAxis,
                        Vector3d.ZAxis,
                        acVportTblRec.Ucs.Origin,
                        acVportTblRec.Ucs.Xaxis,
                        acVportTblRec.Ucs.Yaxis,
                        acVportTblRec.Ucs.Zaxis);

                    pPt3dWCS = pPt3dWCS.TransformBy(newMatrix);

                    Application.ShowAlertDialog("The WCS coordinates are: \n" +
                                                pPt3dWCS.ToString() + "\n" +
                                                "The UCS coordinates are: \n" +
                                                pPt3dUCS.ToString());
                }

                // Save the new objects to the database
                acTrans.Commit();
            }
        }

        public static Point3d ToWCS(this Point3d pUCS)
        {
            if (IsUCSInited == false)
            {
                UCS = GetUCS();
                matrixToUCS = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                    Vector3d.XAxis,
                    Vector3d.YAxis,
                    Vector3d.ZAxis,
                    UCS.Origin,
                    UCS.Xaxis,
                    UCS.Yaxis,
                    UCS.Zaxis);
                IsUCSInited = true;
            }
            Point3d pPt3dWCS= pUCS.TransformBy(matrixToUCS);
            return pPt3dWCS;
        }

        private static bool IsUCSInited = false;
        private static CoordinateSystem3d UCS;
        private static Matrix3d matrixToUCS;
        private static Matrix3d matrixToWCS;

        public static Point3d ToUCS(this Point3d pWCS)
        {
            if (IsUCSInited==false)
            {
                UCS = GetUCS();
                matrixToUCS = Matrix3d.AlignCoordinateSystem(
                    UCS.Origin,
                    UCS.Xaxis,
                    UCS.Yaxis,
                    UCS.Zaxis,
                    Point3d.Origin,
                    Vector3d.XAxis,
                    Vector3d.YAxis,
                    Vector3d.ZAxis);
                IsUCSInited = true;
            }
            Point3d pPt3dUCS = pWCS.TransformBy(matrixToUCS);
            return pPt3dUCS;
        }

        public static CoordinateSystem3d GetUCS()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                    OpenMode.ForWrite) as ViewportTableRecord;
                return acVportTblRec.Ucs;
            }
        }

        public static void ShowWCSAndUCS(Point3d p)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                Point3d pPt3dWCS = p;
                Point3d pPt3dUCS = p;

                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                    OpenMode.ForWrite) as ViewportTableRecord;

                Matrix3d newMatrix = new Matrix3d();
                newMatrix = Matrix3d.AlignCoordinateSystem(Point3d.Origin,
                    Vector3d.XAxis,
                    Vector3d.YAxis,
                    Vector3d.ZAxis,
                    acVportTblRec.Ucs.Origin,
                    acVportTblRec.Ucs.Xaxis,
                    acVportTblRec.Ucs.Yaxis,
                    acVportTblRec.Ucs.Zaxis);

                pPt3dWCS = pPt3dWCS.TransformBy(newMatrix);

                Application.ShowAlertDialog("The WCS coordinates are: \n" +
                                            pPt3dWCS.ToString() + "\n" +
                                            "The UCS coordinates are: \n" +
                                            pPt3dUCS.ToString());

                acTrans.Commit();
            }
        }
        public static void TextReport(string title, string content, double width, double height, bool modal = false, bool writeFile = true)
        {
            try
            {
                string[] list = new string[] { "D", "E", "F", "G", "H" };//C盘没有权限
                string path = "";
                string name = title;
                name = name.Replace("\\", "_");
                name = name.Replace("/", "_");
                name = name.Replace(":", "_");
                name = name.Replace("*", "_");
                name = name.Replace("?", "_");
                name = name.Replace("\"", "_");
                name = name.Replace("<", "_");
                name = name.Replace(">", "_");
                name = name.Replace("|", "_");
                for (int i = 0; i < list.Length; i++)
                {
                    if (Directory.Exists(list[i] + ":\\"))
                    {
                        path = list[i] + ":\\TextReport\\" + name + ".txt";
                        break;
                    }
                }

                FileInfo fi = new FileInfo(path);
                if (fi.Directory.Exists == false)
                {
                    fi.Directory.Create();
                }

                if (writeFile)
                {
                    File.WriteAllText(path, content);
                    content = path + "\n" + content;
                }

                Gui.TextReport(title, content, width, height, modal);
            }
            catch (System.Exception ex)
            {
                Gui.TextReport("错误", ex.ToString(), width, height, modal);
            }

        }
    }




}
