namespace min_edit_distance
{
    /*
     * Cell object meant to populate a 2D array returned by
     * the minEditDistance method. Contains one numeric value
     * as well as three booleans to act as markers for the corresponding
     * cells from which the cell's value was derived.
     **/
    class Cell
    {
        private bool left = false;
        private bool diagonal = false;
        private bool bottom = false;
        private int value;

        public Cell(bool left, bool diagonal, bool bottom, int value)
        {
            this.diagonal = diagonal;
            this.left = left;
            this.bottom = bottom;
            this.value = value;
        }

        public bool Left
        {
            get { return left; }
            set { left = value; }
        }

        public bool Diagonal
        {
            get { return diagonal; }
            set { diagonal = value; }
        }

        public bool Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
