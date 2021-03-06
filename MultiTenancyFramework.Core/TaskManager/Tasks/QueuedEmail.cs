﻿using MultiTenancyFramework.Entities;
using System;

namespace MultiTenancyFramework.Core.TaskManager.Tasks
{
    public class QueuedEmail : BaseEntity
    {
        /// <summary>
        /// Gets or sets the From property (email address)
        /// </summary>
        public virtual string Sender { get; set; }

        /// <summary>
        /// Gets or sets the Sender Display Name property
        /// </summary>
        public virtual string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the To receiver(s) (email address(es)). If more than one, separate with , or ;
        /// </summary>
        public virtual string Receivers { get; set; }

        /// <summary>
        /// Gets or sets the Sender Display Name property
        /// </summary>
        public virtual string ReceiverName { get; set; }

        /// <summary>
        /// Gets or sets the ReplyTo property (email address)
        /// </summary>
        public virtual string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the ReplyToName property
        /// </summary>
        public virtual string ReplyToName { get; set; }

        /// <summary>
        /// Gets or sets the CC
        /// </summary>
        public virtual string CC { get; set; }

        /// <summary>
        /// Gets or sets the BCC
        /// </summary>
        public virtual string Bcc { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public virtual string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Gets or sets the attachment file path (full file path)
        /// </summary>
        public virtual string AttachmentFilePath { get; set; }

        /// <summary>
        /// Gets or sets the attachment content identifier (for use when attaching to email).
        /// </summary>
        public virtual string AttachmentContentId { get; set; }

        /// <summary>
        /// Gets or sets the attachment file name. If specified, then this file name will be sent to a recipient. Otherwise, "AttachmentFilePath" name will be used.
        /// </summary>
        public virtual string AttachmentFileName { get; set; }

        /// <summary>
        /// Gets or sets the download identifier of attached file
        /// </summary>
        public virtual int AttachedDownloadId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of item creation in UTC
        /// </summary>
        public virtual DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the date and time in UTC before which this email should not be sent
        /// </summary>
        public virtual DateTime? DontSendBeforeDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the send tries
        /// </summary>
        public virtual int SentTries { get; set; } = 3;

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        public virtual DateTime? SentOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the used email account identifier
        /// </summary>
        public virtual long EmailAccountId { get; set; }

        public virtual EmailAccount EmailAccount { get; set; }

        private EmailAccount _emailAccount;

        /// <summary>
        /// Gets the email account
        /// </summary>
        public virtual EmailAccount GetEmailAccount()
        {
            return _emailAccount;
        }

        /// <summary>
        /// Sets the email account.
        /// </summary>
        /// <param name="emailAccount">The email account.</param>
        public virtual void SetEmailAccount(EmailAccount emailAccount)
        {
            _emailAccount = emailAccount;
            if (emailAccount != null)
                EmailAccountId = emailAccount.Id;
        }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public virtual QueuedEmailPriority Priority { get; set; }
    }

    /// <summary>
    /// Represents priority of queued email
    /// </summary>
    public enum QueuedEmailPriority
    {
        /// <summary>
        /// Low
        /// </summary>
        Low = 0,
        /// <summary>
        /// High
        /// </summary>
        High = 5
    }
}
