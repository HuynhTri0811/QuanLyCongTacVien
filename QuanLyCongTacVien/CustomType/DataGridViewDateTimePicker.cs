using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace QuanLyCongTacVien.CustomType
{
    // Editing control
    public class DataGridViewDateTimePickerEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public DataGridViewDateTimePickerEditingControl()
        {
            this.Format = DateTimePickerFormat.Short;
        }

        public object EditingControlFormattedValue
        {
            // return the actual DateTime value so DataGridView can assign it to the bound property
            get => this.Value;
            set
            {
                if (value == null) return;
                // accept DateTime or string
                if (value is DateTime dt)
                {
                    this.Value = dt;
                }
                else if (value is string s && DateTime.TryParse(s, out var dt2))
                {
                    this.Value = dt2;
                }
            }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            // return the DateTime value; DataGridView will convert based on cell ValueType/Format
            return this.Value;
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
        }

        public DataGridView EditingControlDataGridView
        {
            get => dataGridView;
            set => dataGridView = value;
        }

        public int EditingControlRowIndex
        {
            get => rowIndex;
            set => rowIndex = value;
        }

        public bool EditingControlValueChanged
        {
            get => valueChanged;
            set => valueChanged = value;
        }

        public Cursor EditingPanelCursor => base.Cursor;

        public bool RepositionEditingControlOnValueChange => false;

        protected override void OnValueChanged(EventArgs eventargs)
        {
            valueChanged = true;
            // Notify the DataGridView that the contents have changed so it can update the cell value
            if (this.EditingControlDataGridView != null)
            {
                // Mark the cell as dirty and immediately commit the edit so the underlying
                // cell value is updated as soon as the user changes the date in the picker.
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
            base.OnValueChanged(eventargs);
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needed
        }
    }

    // Cell
    public class DataGridViewDateTimePickerCell : DataGridViewTextBoxCell
    {
        public DataGridViewDateTimePickerCell()
        {
            this.Style.Format = "d";
        }

        public override Type EditType => typeof(DataGridViewDateTimePickerEditingControl);

        // support nullable DateTime to match DTO which may have DateTime?
        public override Type ValueType => typeof(DateTime?);

        public override object DefaultNewRowValue => null;

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var ctl = DataGridView.EditingControl as DataGridViewDateTimePickerEditingControl;
            if (ctl == null) return;
            if (this.Value == null || this.Value == DBNull.Value)
            {
                // leave uninitialized or set to today
                ctl.Value = DateTime.Now;
            }
            else
            {
                try
                {
                    ctl.Value = Convert.ToDateTime(this.Value);
                }
                catch
                {
                    ctl.Value = DateTime.Now;
                }
            }
        }
    }

    // Column
    public class DataGridViewDateTimePickerColumn : DataGridViewColumn
    {
        public DataGridViewDateTimePickerColumn() : base(new DataGridViewDateTimePickerCell()) { }

        [Browsable(true)]
        public DataGridViewCellStyle DefaultCellStyleDate
        {
            get => this.DefaultCellStyle;
            set => this.DefaultCellStyle = value;
        }
    }
}
