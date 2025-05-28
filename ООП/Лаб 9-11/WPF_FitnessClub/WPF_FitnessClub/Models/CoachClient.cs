using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_FitnessClub.Models
{
    /// <summary>
    /// Модель связи между тренером и клиентом
    /// </summary>
    [Table("CoachClients")]
    public class CoachClient
    {
        // Составной ключ из CoachId и ClientId
        // Id либо отсутствует в таблице, либо имеет другое имя
        
        [Key, Column(Order = 0)]
        [Required]
        public int CoachId { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        public int ClientId { get; set; }

        [ForeignKey("CoachId")]
        public virtual User Coach { get; set; }

        [ForeignKey("ClientId")]
        public virtual User Client { get; set; }

        // Дата добавления клиента
        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }
} 