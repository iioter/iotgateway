namespace Modbus.Data
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    ///     A 1 origin collection represetative of the Modbus Data Model.
    /// </summary>
    public class ModbusDataCollection<TData> : Collection<TData>
    {
        private bool _allowZeroElement = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModbusDataCollection&lt;TData&gt;" /> class.
        /// </summary>
        public ModbusDataCollection()
        {
            AddDefault(this);
            _allowZeroElement = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModbusDataCollection&lt;TData&gt;" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ModbusDataCollection(params TData[] data)
            : this((IList<TData>)data)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ModbusDataCollection&lt;TData&gt;" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public ModbusDataCollection(IList<TData> data)
            : base(AddDefault(data.IsReadOnly ? new List<TData>(data) : data))
        {
            _allowZeroElement = false;
        }

        internal ModbusDataType ModbusDataType { get; set; }

        /// <summary>
        ///     Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"></see> at the specified
        ///     index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     index is less than zero.-or-index is greater than
        ///     <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.
        /// </exception>
        protected override void InsertItem(int index, TData item)
        {
            if (!_allowZeroElement && index == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    "0 is not a valid address for a Modbus data collection.");
            }

            base.InsertItem(index, item);
        }

        /// <summary>
        ///     Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     index is less than zero.-or-index is greater than
        ///     <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.
        /// </exception>
        protected override void SetItem(int index, TData item)
        {
            if (index == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    "0 is not a valid address for a Modbus data collection.");
            }

            base.SetItem(index, item);
        }

        /// <summary>
        ///     Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     index is less than zero.-or-index is equal to or greater than
        ///     <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.
        /// </exception>
        protected override void RemoveItem(int index)
        {
            if (index == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), 
                    "0 is not a valid address for a Modbus data collection.");
            }

            base.RemoveItem(index);
        }

        /// <summary>
        ///     Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
        /// </summary>
        protected override void ClearItems()
        {
            _allowZeroElement = true;
            base.ClearItems();
            AddDefault(this);
            _allowZeroElement = false;
        }

        /// <summary>
        ///     Adds a default element to the collection.
        /// </summary>
        /// <param name="data">The data.</param>
        private static IList<TData> AddDefault(IList<TData> data)
        {
            data.Insert(0, default(TData));
            return data;
        }
    }
}
