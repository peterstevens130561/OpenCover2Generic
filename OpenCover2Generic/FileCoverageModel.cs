using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class FileCoverageModel : IFileCoverageModel
    {
        private readonly string filePath;
        private readonly Dictionary<string,ISequencePoint> coveragePoints = new Dictionary<string,ISequencePoint>();
        private readonly Dictionary<string, IBranchPointAggregator> branchPoints = new Dictionary<string, IBranchPointAggregator>();
        public FileCoverageModel(string filePath)
        {
            this.filePath = filePath;
        }

        public string FullPath
        {
            get
            {
                return filePath;
            }
        }

        public IList<ISequencePoint> SequencePoints
        {
            get
            {
                List<ISequencePoint> points = coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLine.CompareTo(pair2.SourceLine));
                return points;
            }
        }

        public void AddSequencePoint(string sourceLine, string visitedCount)
        {
            coveragePoints.Add(sourceLine, new SequencePoint(sourceLine, visitedCount));
        }


        public void AddBranchPoint(string sourceLine, IBranchPoint branchPoint)
        {
            if (!branchPoints.ContainsKey(sourceLine))
            {
                branchPoints[sourceLine] = new BranchPointAggregator();
            }
            branchPoints[sourceLine].Add(branchPoint);
        }

        public IBranchPointAggregator BranchPointAggregator(string sourceLine)
        {
            return branchPoints.ContainsKey(sourceLine)?branchPoints[sourceLine]:null;
        }

        public void AddBranchPoint(string sourceLine, string IBranchPoint)
        {
            throw new NotImplementedException();
        }

        public void AddBranchPoint(IBranchPoint branchPoint)
        {
            string sourceLine = branchPoint.SourceLine.ToString();
            if (!branchPoints.ContainsKey(sourceLine))
            {
                branchPoints[sourceLine] = new BranchPointAggregator();
            }
            branchPoints[sourceLine].Add(branchPoint);
        }
    }
}