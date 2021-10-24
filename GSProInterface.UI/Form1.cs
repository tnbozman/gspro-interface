using GSProInterface.Adapters;
using GSProInterface.Models.enums;
using GSProInterface.Models.Reponse;
using GSProInterface.Models.Request;
using GSProInterface.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSProInterface.UI
{
    public partial class GSPRO_INTERFACE : Form
    {
        private readonly IGSProInterface _app;
        private readonly ILogger<GSPRO_INTERFACE> _logger;
        public GSPRO_INTERFACE(IGSProInterface app, ILogger<GSPRO_INTERFACE> logger)
        {
            _app = app;
            _app.PlayerInformationReceived += OnPlayerInformationChange;
            _app.ClientConnected += OnConnected;
            _app.ClientDisconnected += OnDisconnected;
            _app.ErrorDetected += OnError;
            _logger = logger;
            InitializeComponent();
        }

        private void connect_Click(object sender, EventArgs e)
        {
            //this.ip_address.Text;
            //this.port.Text;
            if(_app.Status == Status.Disconnected)
            {
                this.connection_status.Text = "Connecting...";
                _app.StartClient();
            }
            else
            {
                this.connection_status.Text = "Disconnecting...";
                _app.StopClient();
            }
            
            
        }

        private void OnError(IGSProInterface intf, string errorMessage)
        {
            this.log.AppendText($"{errorMessage}\n");
        }

        private void OnConnected(IGSProInterface intf)
        {
            this.connection_status.Text = "Connected";
            this.connect.Text = "Disconnect";
        }

        private void OnDisconnected(IGSProInterface intf)
        {
            this.connection_status.Text = "Disconnected";
            this.connect.Text = "Connect";
        }

        private void OnPlayerInformationChange(IGSProInterface intf, ResponseDto response){
            if(response != null && response.Player != null)
            {
                if (!string.IsNullOrEmpty(response.Player.Club))
                    this.club_selection.Text = response.Player.Club;
                if (!string.IsNullOrEmpty(response.Player.Handed))
                    this.player_handed.Text = response.Player.Handed;

            }
        }

        private void lm_status_CheckedChanged(object sender, EventArgs e)
        {
            _app.SendLaunchMonitorStatus(this.lm_status.Checked);
        }

        private void hit_shot_Click(object sender, EventArgs e)
        {
            BallDataDto ballData = null;
            ClubDataDto clubData = null;
            if (this.use_ball_data.Checked)
            {
                ballData = ShotAdapter.WinformToBallData(speed: this.ball_speed.Value,
                                                                        spinAxis: this.ball_spin_axis.Value,
                                                                        totalSpin: this.ball_total_spin.Value,
                                                                        hla: this.ball_hla.Value,
                                                                        vla: this.ball_vla.Value,
                                                                        carry: this.ball_carry_distance.Value);
            }
            if (this.use_club_data.Checked)
            {
                clubData = ShotAdapter.WinformToClubData( speed: this.club_speed.Value,
                                                                        aoa: this.club_aoa.Value,
                                                                        ftt: this.club_ftt.Value,
                                                                        lie: this.club_lie.Value,
                                                                        loft: this.club_loft.Value,
                                                                        path: this.club_path.Value,
                                                                        speedAtImpact: this.club_speed_at_impact.Value,
                                                                        vfi: this.club_vertical_face_impact.Value,
                                                                        hfi: this.club_horizontal_face_impact.Value,
                                                                        closureRate: this.club_closure_rate.Value);
            }

            HitShot(ballData, clubData);
        }

        private void HitShot(BallDataDto ballData, ClubDataDto clubData)
        {
            this.shot_result.Text = "Pending";
            ResponseDto response = null; 
            if(ballData != null && clubData != null)
            {
                response = _app.SendBallAndClubData(ballData, clubData);
            }else if(ballData != null)
            {
                response = _app.SendBallData(ballData);
            }else if(clubData != null)
            {
                response = _app.SendClubData(clubData);
            }

            HandleReponse(response);

        }

        private void HandleReponse(ResponseDto response)
        {
            if(response == null)
            {
                this.shot_result_label.Text = "No Selection";
            }else if(response.Code == (int)ResponseCodes.SHOT_SUCCESS)
            {
                this.shot_result_label.Text = "Success";
            }
        }

    }
}
