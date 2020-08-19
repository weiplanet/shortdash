﻿using System.Collections.Generic;

namespace ShortDash.Server.Data
{
    public class GridCell 
    {
        public string Title { get; set; }
    }

    public class GridRow
    {
        public readonly List<GridCell> Cells;

        public GridRow()
        {
            Cells = new List<GridCell>();
        }
    }
    public class DashModel
    {
        public string Title { get; set; }

        public readonly List<GridRow> Rows;

        public DashModel()
        {
            Rows = new List<GridRow>();
        }
    }
}