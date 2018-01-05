using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    internal class SourceFileCoverageEntity : ISourceFileCoverageModel
    {
        private readonly Dictionary<string,ISequencePointEntity> _coveragePoints = new Dictionary<string,ISequencePointEntity>();
        private readonly Dictionary<string, IBranchPoints> _branchPoints = new Dictionary<string, IBranchPoints>();

        public SourceFileCoverageEntity(string uid,string filePath)
        {
            Uid = uid;
            FullPath = filePath;
        }

        public string FullPath { get; }

        public string Uid { get; }

        public IList<ISequencePointEntity> SequencePoints
        {
            get
            {
                List<ISequencePointEntity> points = _coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLine.CompareTo(pair2.SourceLine));
                return points;
            }
        }

        public void AddSequencePoint(string sourceLine, string visitedCount)
        {
            bool isVisited = int.Parse(visitedCount) > 0;
            if (!_coveragePoints.ContainsKey(sourceLine))
            {
                _coveragePoints[sourceLine]=new SequencePointEntityEntity(sourceLine);
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