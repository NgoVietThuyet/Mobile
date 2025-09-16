using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DMS.CORE.Common;

namespace DMS.CORE.Entities.MT
{
    [Table("T_MT_CHAT")]
    public class TblMtChat : BaseEntity
    {
        [Key]
        [Column("ID")]
        public string? Id { get; set; }

        [Column("MEETING_ID")]
        public string? MeetingId { get; set; }

        [Column("SENDER_ID")]
        public string? SenderId { get; set; }

        [Column("SENDER_USERNAME")]
        public string? SenderUsername { get; set; }

        [Column("SENDER_NAME")]
        public string? SenderName { get; set; }

        /// <summary>
        /// P = Public, D = Direct
        /// </summary>
        [Column("MESSAGE_TYPE")]
        public string? MessageType { get; set; }

        [Column("RECEIVER_ID")]
        public string? ReceiverId { get; set; }

        [Column("RECEIVER_USERNAME")]
        public string? ReceiverUsername { get; set; }

        [Column("RECEIVER_NAME")]
        public string? ReceiverName { get; set; }

        [Column("CONTENT")]
        public string? Content { get; set; }

        [Column("CONTENT_TYPE")]
        public string? ContentType { get; set; } = "TEXT";

        [Column("FILE_PATH")]
        public string? FilePath { get; set; }

        [Column("FILE_NAME")]
        public string? FileName { get; set; }

        [Column("FILE_SIZE")]
        public long? FileSize { get; set; }

        [Column("REPLY_TO_MESSAGE_ID")]
        public string? ReplyToMessageId { get; set; }

        [Column("IS_EDITED")]
        public bool IsEdited { get; set; } = false;

        [Column("IS_DELETED")]
        public bool IsDeleted { get; set; } = false;

        [Column("SENT_TIME")]
        public DateTime SentTime { get; set; } = DateTime.Now;


    }
}
