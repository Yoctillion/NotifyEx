using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyEx.Models
{
    public class req_air_corps_set_plane
    {
        public int api_distance { get; set; }
        public api_plane_info[] api_plane_info { get; set; }
        public int api_after_bauxite { get; set; }
    }
}
