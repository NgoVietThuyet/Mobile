using DMS.CORE.Entities.MT;

namespace DMS.BUSINESS.Models
{
    public class VotesModels
    {
        public TblMtVotes Votes { set; get; }
        public List<VotesQuestionsModels> VoteQuestions { set; get; }
        public TblMtVoteReport? VoteReport { set; get; }  
        public List<TblMtVoteResult>? VoteResult { set; get; } = new List<TblMtVoteResult>();
        public List<string>? IdsQuestionDelete { get; set; }
    }

    public class VotesQuestionsModels
    {
        public TblMtVoteQuestion Config { set; get; }

        public List<TblMtVoteOption> VoteOptions { set; get; }

        public List<string>? IdsOptionDelete { get; set; }
    }

    public class VoteJoinRequest
    {
        public string VoteId { get; set; }
    }

    public class ResultVoteRequest
    {
        public string VoteId { get; set; }
        // public string? UserId { get; set; }

        public List<VoteResponseRequest> Result { get; set; }
    }

    public class OptionSelectRequest
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }

    public class VoteResponseRequest
    {
        public string QuestionId { get; set; }
        public List<OptionSelectRequest>? OptionsSelected { get; set; } // có thể rỗng nếu là text hoặc đánh giá
        public string? ResponseText { get; set; }
    }
}