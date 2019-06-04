using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloBot.Core
{
    public class Constants
    {
        //Categories
        public const string CAT_OTHER = "other";
        public const string CAT_BEATSABER = "beatsaber";
        public const string CAT_KAAMELOTT = "kaamelott";
        public const string CAT_WEATHER = "weather";
        public const string CAT_CHROMECAST = "chromecast";

        //Commands
        public const string CMD_PRONO = "!prono";
        public const string CMD_SLITHER = "!slither";
        public const string CMD_ALEXIS = "!alexis";
        public const string CMD_YAYA = "!yaya";
        public const string CMD_UPTIME = "!uptime";
        public const string CMD_ROLL = "!roll";
        public const string CMD_PUDDY = "!puddy";
        public const string CMD_HORSE = "!horse";
        public const string CMD_POIREAU = "!poireau";
        public const string CMD_SHARK = "!shark";
        public const string CMD_STARS = "!stars";
        public const string CMD_CHICKEN = "!chicken";
        public const string CMD_TAUPE = "!taupe";
        public const string CMD_FROG = "!frog";
        public const string CMD_GITHUB = "!github";
        public const string CMD_SCRIBE = "!scribe";
        public const string CMD_STOP = "!stop";
        public const string CMD_TODAY = "!today";
        public const string CMD_LOVE = "!love";
        public const string CMD_TOMORROW = "!tomorrow";
        public const string CMD_YESTERDAY = "!yesterday";
        public const string CMD_FUTURES = "!futures";
        public const string CMD_TEAMS = "!teams";
        public const string CMD_YOUTUBE = "!yt";
        public const string CMD_ROCKETLEAGUE = "!rl";
        public const string CMD_ROCKETLEAGUE_APOLLO = "!rlapollo";
        public const string CMD_ROCKETLEAGUE_MEHDI = "!rlsopalin";
        public const string CMD_WEATHER_WEEK = "!meteoweek";
        public const string DESC_WEATHER_WEEK = "Affiche la météo de la semaine";
        public const string CMD_WEATHER_DAY = "!meteoday";
        public const string DESC_WEATHER_DAY = "Affiche la météo de la journée";
        public const string CMD_KAAMELOTT_SEARCH = "!k";
        public const string DESC_KAAMELOTT_SEARCH = "Cherche une réplique de Kaamelott";
        public const string CMD_KAAMELOTT_SITE = "!ksite";
        public const string DESC_KAAMELOTT_SITE = "Donne le lien regroupant les répliques de Kaamelott";
        public const string CMD_KAAMELOTT_PLAY = "!kp";
        public const string DESC_KAAMELOTT_PLAY = "Joue une réplique de Kaamelott";
        public const string CMD_KAAMELOTT_VOLUME = "!kv";
        public const string DESC_KAAMELOTT_VOLUME = "Change le volume de la réplique";
        public const string CMD_KAAMELOTT_LIST = "!klist";
        public const string DESC_KAAMELOTT_LIST = "Liste toutes les répliques de Kaamelott";
        public const string CMD_CHROMECAST_FIND = "!castfind";
        public const string DESC_CHROMECAST_FIND = "Cherche les chromecasts disponibles";
        public const string CMD_CHROMECAST_SELECT = "!castselect";
        public const string DESC_CHROMECAST_SELECT = "Sélectionne un chromecast";
        public const string CMD_CHROMECAST_SEND = "!ccs";
        public const string DESC_CHROMECAST_SEND = "Envoie d'un flus au chromecast";
        public const string CMD_CHROMECAST_STOP = "!ccstop";
        public const string DESC_CHROMECAST_STOP = "Arrêt chromecast";
        public const string CMD_LIST = "!list";
        public const string CMD_BEATSAVER_LASTEST = "!beatsaberlist";
        public const string DESC_BEATSAVER_LASTEST = "Dernières musiques ajoutées";
        public const string CMD_BEATSAVER_TOP_DOWNLOAD = "!beatsabertopdownload";
        public const string DESC_BEATSAVER_TOP_DOWNLOAD = "Top téléchargés";
        public const string CMD_BEATSAVER_TOP_PLAYED= "!beatsabertopplayed";
        public const string DESC_BEATSAVER_TOP_PLAYED= "Top joués";
        public const string CMD_BEATSAVER_SEARCH = "!beatsabersearch";
        public const string DESC_BEATSAVER_SEARCH = "Chercher une musique";
        public const string CMD_BEATSAVER_PLAY = "!bsp";
        public const string DESC_BEATSAVER_PLAY = "Jouer une musique";
        public const string CMD_BEATSAVER_STOP = "!bsstop";
        public const string DESC_BEATSAVER_STOP = "Arreter la musique";

        public const string DESC_NO = "Pas de description";

        //Readers
        public const string CMD_READER_SET = "!setReader";
        public const string DESC_READER_SET = "Change le lecteur";
        public const string CMD_READER_GET = "!getReader";
        public const string DESC_READER_GET = "Récupère le lecteur courant";
        public const string CMD_READER_GET_ALL = "!getReaders";
        public const string DESC_READER_GET_ALL = "Récupère tous les lecteurs disponibles";

        //Categories
        public const string CMD_CATEGORY_ACTIONS = "!category";
        public const string DESC_CATEGORY_ACTIONS = "Récupère toutes les actions d'une categorie";
        public const string CMD_CATEGORY_ALL = "!categories";
        public const string DESC_CATEGORY_ALL = "Récupère toutes les categories";

        //Format
        public const string DATETIME_FORMAT = "ddd dd/MM HH:mm";
        public const string DATE_FORMAT = "ddd dd/MM";
        public const string TIME_FORMAT = "HH:mm";
    }
}
