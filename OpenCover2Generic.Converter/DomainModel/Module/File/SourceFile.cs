using System.Collections.Generic;
using System.Linq;
using BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File.Line;

namespace BHGE.SonarQube.OpenCover2Generic.DomainModel.Module.File
{
    internal class SourceFile : ISourceFile
    {
        private readonly Dictionary<string,ISequencePoint> _coveragePoints = new Dictionary<string,ISequencePoint>();
        private readonly Dictionary<string, IBranchPoints> _branchPoints = new Dictionary<string, IBranchPoints>();

        public SourceFile(string uid,string filePath)
        {
            Uid = uid;
            FullPath = filePath;
        }

        public string FullPath { get; }

        public string Uid { get; }

        public IList<ISequencePoint> SequencePoints
        {
            get
            {
                List<ISequencePoint> points = _coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLineId.CompareTo(pair2.SourceLineId));
                return points;
            }
        }

        public void AddSequencePoint(string sourceLine, string visitedCount)
        {
            bool isVisited = int.Parse(visitedCount) > 0;
            if (!_coveragePoints.ContainsKey(sourceLine))
            {
                _coveragePoints[sourceLine]=new SequencePoint(sourceLine);
            }
            _coveragePoints[sourceLine].AddVisit(isVisited);
        }

        public IBranchPoints GetBranchPointsByLine(string sourceLine)
        {
            return _branchPoints.ContainsKey(sourceLine)?_branchPoints[sourceLine]:null;
        }

        public void AddBranchPoint(IBranchPoint branchPoint)
        {
            string sourceLine = branchPoint.SourceLine.ToString();
            if (!_branchPoints.ContainsKey(sourceLine))
            {
                _branchPoints[sourceLine] = new BranchPoints();
            }
            _branchPoints[sourceLine].Add(branchPoint);
        }
    }
}