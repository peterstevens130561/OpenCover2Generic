using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic.Model
{
    internal class SourceFileCoverageModel : ISourceFileCoverageModel
    {
        private readonly string _filePath;
        private readonly Dictionary<string,ISequencePoint> _coveragePoints = new Dictionary<string,ISequencePoint>();
        private readonly Dictionary<string, IBranchPoints> _branchPoints = new Dictionary<string, IBranchPoints>();
        private readonly string _uid;

        public SourceFileCoverageModel(string uid,string filePath)
        {
            _uid = uid;
            this._filePath = filePath;
        }

        public string FullPath
        {
            get
            {
                return _filePath;
            }
        }

        public string Uid
        {
            get
            {
                return _uid;
            }
        }
        public IList<ISequencePoint> SequencePoints
        {
            get
            {
                List<ISequencePoint> points = _coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLine.CompareTo(pair2.SourceLine));
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


        public void AddBranchPoint(string sourceLine, IBranchPoint branchPoint)
        {
            if (!_branchPoints.ContainsKey(sourceLine))
            {
                _branchPoints[sourceLine] = new BranchPoints();
            }
            _branchPoints[sourceLine].Add(branchPoint);
        }

        public IBranchPoints GetBranchPointAggregatorByLine(string sourceLine)
        {
            return _branchPoints.ContainsKey(sourceLine)?_branchPoints[sourceLine]:null;
        }

        public void AddBranchPoint(string sourceLine, string IBranchPoint)
        {
            throw new NotImplementedException();
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