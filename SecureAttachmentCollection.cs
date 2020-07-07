using System;
using System.Collections;
using System.Collections.Generic;

namespace VNetDev.SecureMail
{
    /// <summary>
    /// SecureMailAttachment collection
    /// </summary>
    public class SecureAttachmentCollection : ICollection<SecureAttachment>
    {
        private readonly List<SecureAttachment> _attachments;

        /// <summary>
        /// Creates SecureMailAttachment collection
        /// </summary>
        public SecureAttachmentCollection()
        {
            _attachments = new List<SecureAttachment>();
        }

        /// <summary>
        /// Returns enumerator of SecureAttachments
        /// </summary>
        /// <returns>Enumerator of SecureAttachment</returns>
        public IEnumerator<SecureAttachment> GetEnumerator() => _attachments.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Adds SecureAttachment to the collection
        /// </summary>
        /// <param name="item">SecureAttachment instance</param>
        public void Add(SecureAttachment item)
        {
            if (!Contains(item))
                _attachments.Add(item);
        }

        /// <summary>
        /// Removes all attachments from the collection
        /// </summary>
        public void Clear() => _attachments.Clear();

        /// <summary>
        /// Checks either collection contains given attachment or not
        /// </summary>
        /// <param name="item">SecureAttachment instance</param>
        /// <returns>true if contains specified object otherwise false</returns>
        public bool Contains(SecureAttachment item) => _attachments.Contains(item);

        /// <summary>
        /// Always throws <c>NotSupportedException</c>
        /// </summary>
        public void CopyTo(SecureAttachment[] array, int arrayIndex) =>
            throw new NotSupportedException(
                $"{nameof(CopyTo)} is not supported by {nameof(SecureAttachmentCollection)}.");

        /// <summary>
        /// Removed given item from the collection
        /// </summary>
        /// <param name="item">SecureAttachment instance</param>
        /// <returns>true if removed otherwise false</returns>
        public bool Remove(SecureAttachment item) => _attachments.Remove(item);

        /// <summary>
        /// Count of collection elements
        /// </summary>
        public int Count => _attachments.Count;

        /// <summary>
        /// Shows if collection is readonly that is always false
        /// </summary>
        public bool IsReadOnly => false;
    }
}