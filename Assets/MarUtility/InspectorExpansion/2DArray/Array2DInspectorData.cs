/*
 * Marlow Greenan
 * Created: 7/21/2026
 * Last Updated: 7/21/2026 by Marlow Greenan
 * 
 * Contains data for 2D arrays in inspectors.
 */
namespace MarUtility.InspectorExtentions
{
    public class Array2DInspectorData{}

    #region Int
    [System.Serializable]
    public class IntCellData
    {
        public RowData[] rows = new RowData[7];

        [System.Serializable]
        public class RowData
        {
            public int[] row = new int[7];
        }
    }
    #endregion

    #region Bool
    [System.Serializable]
    public class BoolCellData
    {
        public RowData[] rows = new RowData[7];

        [System.Serializable]
        public class RowData
        {
            public bool[] row = new bool[7];
        }
    }
    #endregion
}
