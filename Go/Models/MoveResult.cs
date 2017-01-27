using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Models
{
    public class MoveResult
    {
        private bool _isNormalSuccess;
        private bool _isRuleConfilct;
        private bool _isMoveKill;
        public bool IsNormalMoveSuccess
        {
            get
            {
                return _isNormalSuccess;
            }
            set
            {
                _isNormalSuccess = value;
                if (_isNormalSuccess)
                {
                    IsMoveKill = false;
                    IsRuleConflict = false;
                }
            }
        }

        public bool IsMoveKill
        {
            get
            {
                return _isMoveKill;
            }
            set
            {
                _isMoveKill = value;
                if (_isMoveKill)
                {
                    _isNormalSuccess = false;
                    _isRuleConfilct = false;
                }
            }
        }

        public bool IsRuleConflict
        {
            get
            {
                return _isRuleConfilct;
            }
            set
            {
                _isRuleConfilct = value;
                if (_isRuleConfilct)
                {
                    _isNormalSuccess = false;
                    _isMoveKill = false;
                }
            }
        }

        public int KillCount { get; set; }

        public List<Chain> DeadChains { get; set; }
    }
}
