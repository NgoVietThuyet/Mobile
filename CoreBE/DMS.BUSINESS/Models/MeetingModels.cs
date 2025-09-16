using DMS.BUSINESS.Dtos.MT;
using DMS.CORE.Entities.CM;
using DMS.CORE.Entities.MT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.BUSINESS.Models
{
    public class MeetingModels
    {
        public TblMtMeeting MeetInfor { set; get; }
        public List<FileModels>? Files { set; get; } = new List<FileModels>();
        public List<string>? Members { set; get; } = new List<string>();
        public List<string>? Secretaries { set; get; } = new List<string>();
        public string? HostMeeting { set; get; }
        public List<TblMtMeetingMember>? memberMeeting { set; get; } = new List<TblMtMeetingMember>();
        public List<TblMtSeatAssignments>? SeatAssignments { set; get; } = new List<TblMtSeatAssignments>();
        public List<TblMtVotes>? Votes { set; get; } = new List<TblMtVotes>();
    }

    public class FileModels
    {
        public TblMtMeetingFile File { set; get; }
        public List<string>? MemberFile { set; get; } = new List<string>();

    }


    public class MeetingCliModel
    {
        public TblMtMeeting MeetInfor { set; get; }
        public List<TblMtMeetingFile>? PersonalDocuments { set; get; } = new List<TblMtMeetingFile>();
        public List<TblMtMeetingFile> MeetingDocuments { set; get; } = new List<TblMtMeetingFile>();
        public List<TblMtMeetingFile> MeetingDraw { set; get; } = new List<TblMtMeetingFile>();
        public List<TblMtVotes> MeetingVotes { set; get; } = new List<TblMtVotes>();
        public List<TblCmFile> Voices { set; get; } = new List<TblCmFile>();
        public TblCmFile Summary { set; get; }
        public List<TblMtSeatAssignments>? SeatAssignments { set; get; } = new List<TblMtSeatAssignments>();
        public List<MemberMeeting>? MemberMeeting { set; get; } = new List<MemberMeeting>();

    }

    public class MemberMeeting 
    {
        public string? Id { get; set; }
        public string? GuestName { get; set; }
        public string? MeetingId { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public string? Type { get; set; }
        public bool IsCameraOff { get; set; }
        public bool IsMicMuted { get; set; }

    }


    public class CreateFileFromTemplateRequest
    {
        public string FilePath { get; set; }
        public string NewFileName { get; set; }
        public string? UserId { get; set; }
        public string MeetId { get; set; }
    }



    public class BlobFile
    {
        public string MeetingId { get; set; }
        public string UserName { get; set; }
        public IFormFile file { get; set; }
        public string fileName { get; set; }
        public string? filePath { get; set; }
    }
}
