using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyEx.Models
{
    public class base_air_corps
    {
        public int api_rid { get; set; }
        public string api_name { get; set; }
        public int api_distance { get; set; }
        public int api_action_kind { get; set; }
        public api_plane_info[] api_plane_info { get; set; }
    }

    public class api_plane_info
    {
        public int api_squadron_id { get; set; }
        public int api_state { get; set; }
        public int api_slotid { get; set; }
        public int api_count { get; set; }
        public int api_max_count { get; set; }
        public int api_cond { get; set; }
    }
}
