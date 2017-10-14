using System;
using System.Collections.Generic;
using System.Linq;

namespace BHGE.SonarQube.OpenCover2Generic
{
    internal class FileCoverageModel : IFileCoverageModel
    {
        private readonly string filePath;
        private readonly Dictionary<string,ICoveragePoint> coveragePoints = new Dictionary<string,ICoveragePoint>();
        private readonly Dictionary<string, IBranchPoint> branchPoints = new Dictionary<string, IBranchPoint>();
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

        public IList<ICoveragePoint> SequencePoints
        {
            get
            {
                List<ICoveragePoint> points = coveragePoints.Values.ToList();
                points.Sort((pair1, pair2) => pair1.SourceLine.CompareTo(pair2.SourceLine));
                return points;
            }
        }

        public void AddSequencePoint(string sourceLine, string visitedCount)
        {
            Boolean visited = int.Parse(visitedCount) > 0;
            if (coveragePoints.ContainsKey(sourceLine))
            {
                coveragePoints[sourceLine].Add(visited);
            }
            else
            {
                coveragePoints.Add(sourceLine, new CoveragePoint(sourceLine, visited));
            }
        }

        public void AddBranchPoint(string sourceLine, string visitedCount)
        {
            Boolean branchVisited = int.Parse(visitedCount) > 0;
            IBranchPoint branchPoint = new BranchPoint(branchVisited);
            branchPoints[sourceLine] = branchPoints.ContainsKey(sourceLine) ? branchPoints[sourceLine].Add(branchPoint): branchPoint;

        }

        public IBranchPoint BranchPoint(string sourceLine)
        {
            return branchPoints.ContainsKey(sourceLine)?branchPoints[sourceLine]:null;
        }
    }
}