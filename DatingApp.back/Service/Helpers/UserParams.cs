using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Helpers
{
    /// <summary>
    /// Paramètre utilisé dans la construction du filtre
    /// </summary>
    public class UserParams : PaginationParams
    {
        //Permet de retirer la personne connecté de la liste d'affichage
        public string CurrentUsername { get; set; }
        //Faire un opposé du sexe de la personne pour le genre à afficher
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; } = "lastActive";
    }
}
