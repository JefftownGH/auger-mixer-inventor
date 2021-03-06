using System.Collections.Generic;
using Inventor;

namespace AugerMixer.Model
{
    /// <summary>
    /// Contains frequently used API to work with Inventor faster.
    /// </summary>
    class InventorAPI
    {
        private readonly string shortName;
        private Application app = null;
        private static Dictionary<string, string> fileName = new Dictionary<string, string>();
        private static Dictionary<string, PartDocument> partDocument = new Dictionary<string, PartDocument>();
        private static Dictionary<string, PartComponentDefinition> partComponentDefinition = new Dictionary<string, PartComponentDefinition>();
        private static Dictionary<string, TransientGeometry> transientGeometry = new Dictionary<string, TransientGeometry>();
        private AssemblyComponentDefinition assemblyComponentDefinition;
        public InventorAPI(Application app, string shortName, string longName)
        {
            this.shortName = shortName;
            this.app = app;
            partDocument[shortName] = (PartDocument)app.Documents.Add(DocumentTypeEnum.kPartDocumentObject, app.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject));
            partComponentDefinition[shortName] = partDocument[shortName].ComponentDefinition;
            transientGeometry[shortName] = app.TransientGeometry;
            fileName[shortName] = null;
            partDocument[shortName].DisplayName = longName;
        }
        public InventorAPI(AssemblyComponentDefinition assemblyComponentDefinition) =>
            this.assemblyComponentDefinition = assemblyComponentDefinition;
        // Accessing dictionaries
        public string GetShortName() =>
            shortName;
        public string GetLongName() =>
            partDocument[shortName].DisplayName;
        public void SetFileName(string setName) =>
            fileName[shortName] = setName;
        public string GetFileName() =>
            fileName[shortName];
        public PartDocument GetPartDoc() =>
            partDocument[shortName];
        public PartComponentDefinition GetCompDef() =>
            partComponentDefinition[shortName];
        public TransientGeometry GetTransGeom() =>
            transientGeometry[shortName];
        // 2D
        public PlanarSketch Sketch(object plane, bool useFaceEdges = false) =>
            GetCompDef().Sketches.Add(plane, useFaceEdges);
        public Profile Profile(PlanarSketch sketch) =>
            sketch.Profiles.AddForSolid();
        public SketchPoint Point(PlanarSketch sketch, double x, double y, bool holeCenter = false) =>
            sketch.SketchPoints.Add(GetTransGeom().CreatePoint2d(x, y), holeCenter);
        public SketchLine Line(PlanarSketch sketch, SketchPoint point1, SketchPoint point2) =>
            sketch.SketchLines.AddByTwoPoints(point1, point2);
        public SketchCircle Circle(PlanarSketch sketch, SketchPoint point, double radius) =>
            sketch.SketchCircles.AddByCenterRadius(point, radius);
        // 3D
        /// <param name="direction">
        /// 0-Positive;
        /// 1-Negative;
        /// 2-Symmetric;
        /// </param>
        /// <param name="operation">
        /// 0-Join;
        /// 1-Cut;
        /// </param>
        public ExtrudeFeature Extrude(Profile profile, double distance, int direction, int operation)
        {
            PartFeatureExtentDirectionEnum extentDirection;
            switch (direction)
            {
                case 0:
                    extentDirection = PartFeatureExtentDirectionEnum.kPositiveExtentDirection;
                    break;
                case 1:
                    extentDirection = PartFeatureExtentDirectionEnum.kNegativeExtentDirection;
                    break;
                default:
                    extentDirection = PartFeatureExtentDirectionEnum.kSymmetricExtentDirection;
                    break;
            }
            PartFeatureOperationEnum extentOperation;
            switch (operation)
            {
                case 0:
                    extentOperation = PartFeatureOperationEnum.kJoinOperation;
                    break;
                default:
                    extentOperation = PartFeatureOperationEnum.kCutOperation;
                    break;
            }
            return GetCompDef().Features.ExtrudeFeatures.AddByDistanceExtent(profile, distance, extentDirection, extentOperation, profile);
        }
        /// <param name="operation">
        /// 0-Join;
        /// 1-Cut;
        /// </param>
        public RevolveFeature Revolve(Profile profile, object axis, int operation)
        {
            PartFeatureOperationEnum extentOperation;
            switch (operation)
            {
                case 0:
                    extentOperation = PartFeatureOperationEnum.kJoinOperation;
                    break;
                default:
                    extentOperation = PartFeatureOperationEnum.kCutOperation;
                    break;
            }
            return GetCompDef().Features.RevolveFeatures.AddFull(profile, axis, extentOperation);
        }
        // Assembly
        public void Plane(int OccurrenceOne, int PartPlaneOne, int OccurrenceTwo, int PartPlaneTwo, string Offset = "0", bool MateOrFlush = false)
        {
            var oOcc1 = assemblyComponentDefinition.Occurrences[OccurrenceOne];
            var oOcc2 = assemblyComponentDefinition.Occurrences[OccurrenceTwo];
            var oPartComDef = (PartComponentDefinition)oOcc1.Definition;
            var oPartPlane1 = oPartComDef.WorkPlanes[PartPlaneOne];
            oPartComDef = (PartComponentDefinition)oOcc2.Definition;
            var oPartPlane2 = oPartComDef.WorkPlanes[PartPlaneTwo];
            oOcc1.CreateGeometryProxy(oPartPlane1, out object oAsmPlane1Obj);
            var oAsmPlane1 = (WorkPlaneProxy)oAsmPlane1Obj;
            oOcc2.CreateGeometryProxy(oPartPlane2, out object oAsmPlane2Obj);
            var oAsmPlane2 = (WorkPlaneProxy)oAsmPlane2Obj;
            if (MateOrFlush)
                assemblyComponentDefinition.Constraints.AddMateConstraint(oAsmPlane1, oAsmPlane2, Offset);
            else
                assemblyComponentDefinition.Constraints.AddFlushConstraint(oAsmPlane1, oAsmPlane2, Offset);
        }
        public void Axis(int OccurrenceOne, int PartAxisOne, int OccurrenceTwo, int PartAxisTwo, string Offset = "0")
        {
            var oOcc1 = assemblyComponentDefinition.Occurrences[OccurrenceOne];
            var oOcc2 = assemblyComponentDefinition.Occurrences[OccurrenceTwo];
            var oPartComDef = (PartComponentDefinition)oOcc1.Definition;
            var oPartAxis1 = oPartComDef.WorkAxes[PartAxisOne];
            oPartComDef = (PartComponentDefinition)oOcc2.Definition;
            var oPartAxis2 = oPartComDef.WorkAxes[PartAxisTwo];
            oOcc1.CreateGeometryProxy(oPartAxis1, out object oAsmAxis1Obj);
            var oAsmAxis1 = (WorkAxisProxy)oAsmAxis1Obj;
            oOcc2.CreateGeometryProxy(oPartAxis2, out object oAsmAxis2Obj);
            var oAsmAxis2 = (WorkAxisProxy)oAsmAxis2Obj;
            assemblyComponentDefinition.Constraints.AddMateConstraint(oAsmAxis1, oAsmAxis2, Offset);
        }
        public void PlaneAngle(int OccurrenceOne, int PartPlaneOne, int OccurrenceTwo, int PartPlaneTwo, string Offset = "0")
        {
            var oOcc1 = assemblyComponentDefinition.Occurrences[OccurrenceOne];
            var oOcc2 = assemblyComponentDefinition.Occurrences[OccurrenceTwo];
            var oPartComDef = (PartComponentDefinition)oOcc1.Definition;
            var oPartPlane1 = oPartComDef.WorkPlanes[PartPlaneOne];
            oPartComDef = (PartComponentDefinition)oOcc2.Definition;
            var oPartPlane2 = oPartComDef.WorkPlanes[PartPlaneTwo];
            oOcc1.CreateGeometryProxy(oPartPlane1, out object oAsmPlane1Obj);
            var oAsmPlane1 = (WorkPlaneProxy)oAsmPlane1Obj;
            oOcc2.CreateGeometryProxy(oPartPlane2, out object oAsmPlane2Obj);
            var oAsmPlane2 = (WorkPlaneProxy)oAsmPlane2Obj;
            assemblyComponentDefinition.Constraints.AddAngleConstraint(oAsmPlane1, oAsmPlane2, Offset);
        }
        public void Surface(int OccurrenceOne, int PartFaceOne, int OccurrenceTwo, int PartFaceTwo, double Offset = 0, bool MateOrFlush = true)
        {
            var oOcc1 = assemblyComponentDefinition.Occurrences[OccurrenceOne];
            var oOcc2 = assemblyComponentDefinition.Occurrences[OccurrenceTwo];
            var oFace1 = oOcc1.SurfaceBodies[1].Faces[PartFaceOne];
            var oFace2 = oOcc2.SurfaceBodies[1].Faces[PartFaceTwo];
            if (MateOrFlush)
                assemblyComponentDefinition.Constraints.AddMateConstraint(oFace1, oFace2, Offset);
            else
                assemblyComponentDefinition.Constraints.AddFlushConstraint(oFace1, oFace2, Offset);
        }
        // Additional features
        public ObjectCollection ObjectCollection() =>
            app.TransientObjects.CreateObjectCollection();
        public EdgeCollection EdgeCollection() =>
            app.TransientObjects.CreateEdgeCollection();
        public ThreadFeatures ThreadFeatures() =>
            GetCompDef().Features.ThreadFeatures;
    }
}