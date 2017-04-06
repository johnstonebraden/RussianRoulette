using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RussianRouletteAssignment
{
    public partial class frmGame : Form
    {
        Random rnd = new Random();  // declaring variables
        int bulletLoad = 0;
        int bulletsLeft = 6;
        int bulletcount = 0;
        int score = 0;
        int aimAwayChances = 2;
        int round = 1;
        
        public frmGame()
        {
            InitializeComponent();
        }     
          
        private void RussianRoulette_Load(object sender, EventArgs e) 
        //saying hello to the user when they load the game form
        {
            lblHello.Text = "Hello " + frmMenu.userName + "!" + "\nAre you ready for some Russian Roulette?";

            WMPBackgroundSong.URL = System.AppDomain.CurrentDomain.BaseDirectory + "myNameIsBond.WAV";
        }
         
        private void btnBulletLoad_Click(object sender, EventArgs e) 
        // chooses random number for where the bullet isand disables the button so you cannot load more than one bullet
        {
            btnBulletLoad.Text = "LOADED!";
            bulletLoad = rnd.Next(1, 7);
            btnBulletLoad.Enabled = false;
            btnChamberSpin.Enabled = true;          
            lblScore.Text = "Your score: " + score;
            aimAwayChances = 2;
            lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
            SoundPlayer loadBullet = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "AmmoCasing.WAV");
            // plays the sound on button click
            loadBullet.Play();
        }

        private void btnChamberSpin_Click(object sender, EventArgs e)
        {
            pbAimHead.Enabled = true;
            pbAimAway.Enabled = true;
            SoundPlayer chamberSpin = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "ChamberSpin.WAV");  
            // plays the sound on button click
            chamberSpin.Play();
        }

        private void pbAimHead_Click(object sender, EventArgs e)
        {
            if (bulletsLeft <= 6 && bulletsLeft > 1)  //checks if you have bullets left
            {
                if (bulletLoad == bulletcount)  
                // checking if you shoot youself or get blank shot
                {

                    pbAimHead.Enabled = false;
                    pbAimAway.Enabled = false;
                    btnChamberSpin.Enabled = false;
                    bulletsLeft--;
                    lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
                    lblShotOrNot.Text = "You were shot! Game over!";
                    btnNextRound.Enabled = false;
                    btnSaveHighScore.Enabled = true;
                    SoundPlayer gunShot = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "RevolverShot.WAV");                   
                    // plays the sound on button click
                    gunShot.Play();
                    bulletcount = 6;
                    pbAimHead.Image = Properties.Resources.deathImage;
                }
                else if (bulletLoad != bulletcount)
                //if you survive, goes to next bullet check, changes scores
                {
                    bulletsLeft = bulletsLeft - 1;
                    lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
                    bulletcount = bulletcount + 1;
                    lblShotOrNot.Text = "You survive this one!";
                    score = score + 100;
                    lblScore.Text = "Your score: " + score;
                    SoundPlayer blankShot = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "dryFire.WAV");
                    // plays the sound on button click
                    blankShot.Play();
                }
            }
            else if (bulletsLeft <= 1)
            {
                pbAimHead.Enabled = false;
                pbAimAway.Enabled = false;
                btnChamberSpin.Enabled = false;
                bulletsLeft = bulletsLeft - 1;
                lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
                lblShotOrNot.Text = "You were shot! Game over!";
                btnNextRound.Enabled = false;
                btnSaveHighScore.Enabled = true;
                SoundPlayer gunShot = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "RevolverShot.WAV");
                gunShot.Play();
                pbAimHead.Image = Properties.Resources.deathImage;               
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)  
            //restarts the game round, setting your score to zero
        {
            btnBulletLoad.Text = "Load Bullet";
            btnNextRound.Enabled = false;
            btnBulletLoad.Enabled = true;
            btnChamberSpin.Enabled = false;
            score = 0;
            lblScore.Text = "Your score: " + score;
            aimAwayChances = 2;
            lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
            bulletsLeft = 6;
            bulletcount = 0;
            lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
            round = 1;
            lblRound.Text = "Round: " + round;
            //bulletLoad = rnd.Next(1, 7); removed as not needed
            lblShotOrNot.Text = "";
            pbAimHead.Image = Properties.Resources.pokerFace2;
            pbAimAway.Image = Properties.Resources.aimAway;
            pbAimHead.Enabled = false;
            pbAimAway.Enabled = false;
        }

        private void btnNextRound_Click(object sender, EventArgs e) 
            // starts next round (only enabled if you won previous)
        {
            btnBulletLoad.Text = "Load Bullet";
            btnBulletLoad.Enabled = true;
            btnChamberSpin.Enabled = false;
            bulletLoad = rnd.Next(1, 7);
            lblScore.Text = "Your score: " + score;
            aimAwayChances = 2;
            lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
            bulletsLeft = 6;
            lblBulletsLeft.Text = "Bullets left: " + bulletsLeft;
            round = round + 1;
            lblRound.Text = ("Round: " + round);
            pbAimAway.Image = Properties.Resources.aimAway;
            pbAimHead.Enabled = false;
            pbAimAway.Enabled = false;
            btnNextRound.Enabled = false;

        }

        private void btnSaveHighScore_Click(object sender, EventArgs e) 
            //saving the high score
        {
            try
            {
                using (StreamWriter writer = File.AppendText(System.AppDomain.CurrentDomain.BaseDirectory + "HighScores.txt"))
                {
                    writer.WriteLine(frmMenu.userName + "|" + score);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("The file could not be written: " + ex);
            }
            btnSaveHighScore.Enabled = false;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            frmMenu f1 = new frmMenu();
            f1.Show();
            this.Close();
        }

        private void pbAimAway_Click(object sender, EventArgs e)
        {
            if (aimAwayChances == 1)
            //disables the aim away button if you have used it twice and sets your next shot to kill you            
            {
                pbAimAway.Enabled = false;
                bulletLoad = bulletcount; //setting bullet load and bullet count as the same so you lose on next shot
                lblAimAwayChances.Text = "No more chances!";
                pbAimAway.Image = Properties.Resources.noMoreAimAway2;
                bulletsLeft = bulletsLeft - 1;
                lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
                SoundPlayer blankShot = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "dryFire.WAV");
                // plays the sound on button click
                blankShot.Play();
            }

            else if (bulletLoad != bulletcount) //checks if you fire a blank away from you
            {
                aimAwayChances--;
                lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
                lblShotOrNot.Text = "Well that was a waste";
                bulletcount = bulletcount + 1;
                SoundPlayer blankShot = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "dryFire.WAV");
                // plays the sound on button click
                blankShot.Play();
                bulletsLeft = bulletsLeft - 1;
                lblBulletsLeft.Text = "Shots left: " + bulletsLeft;

            }
            else if (bulletLoad == bulletcount)
            // checks if you shoot away from you
            {
                aimAwayChances--;
                lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
                score = score + 500;
                btnBulletLoad.Enabled = false;
                btnChamberSpin.Enabled = false;
                pbAimAway.Enabled = false;
                pbAimHead.Enabled = false;
                btnNextRound.Enabled = true;
                lblShotOrNot.Text = "You win this round!";
                lblScore.Text = "Your score: " + score;
                bulletcount = bulletcount + 1;
                SoundPlayer bulletPass = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "BulletPass.WAV");
                // plays the sound on button click
                bulletPass.Play();
                bulletsLeft = bulletsLeft - 1;
                lblBulletsLeft.Text = "Shots left: " + bulletsLeft;
            }
            if(bulletsLeft <= 1)
            {
                aimAwayChances--;
                lblAimAwayChances.Text = "Aim away chances left: " + aimAwayChances;
                score = score + 500;
                btnBulletLoad.Enabled = false;
                btnChamberSpin.Enabled = false;
                pbAimAway.Enabled = false;
                pbAimHead.Enabled = false;
                btnNextRound.Enabled = true;
                lblShotOrNot.Text = "You win this round!";
                lblScore.Text = "Your score: " + score;
                bulletcount = bulletcount + 1;
                SoundPlayer bulletPass = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "BulletPass.WAV");
                // plays the sound on button click
                bulletPass.Play();
                bulletsLeft = 0;
                lblBulletsLeft.Text = "Shots left: " + bulletsLeft;                
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            string help = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "help.txt");           
            MessageBox.Show(help);
        }

        private void btnMusic_Click(object sender, EventArgs e)// playing song in  background
        {
            MessageBox.Show("Song: My name is bond, By Martijn DeBoer (NiGiD)");
            //SoundPlayer song = new SoundPlayer(System.AppDomain.CurrentDomain.BaseDirectory + "myNameIsBond.WAV");
            //song.PlayLooping();
        }       
    }
}
