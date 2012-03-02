using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GaiaSequel
{
    class MediaPlayerGaia
    {

        String songPlaying;
        String[] songs = new String[29];
        ContentManager conentInfo;
        public void init(ContentManager content){

            this.conentInfo = content;
            songPlaying = songs[0];
            songs[0] = "Intro";
            songs[1] = "SouthCape";
            songs[2] = "MapOfTheWorld";

            MediaPlayer.Play(content.Load<Song>("Music/" + songs[0]));
            MediaPlayer.IsRepeating = true;
            songPlaying = songs[0];
        }
        //This will only play the new song if the current song is not playing
        public void playNew(int song){
            if (!songPlaying.Equals(songs[song]))
            {
                MediaPlayer.Play(conentInfo.Load<Song>("Music/" + songs[song]));
                songPlaying = songs[song];
            }
        }

        public void stopSong(){
            MediaPlayer.Stop();
        }
        public void pause(){
            MediaPlayer.Pause();
        }
        public void resume(){
            MediaPlayer.Resume();
        }
        

    }
}
