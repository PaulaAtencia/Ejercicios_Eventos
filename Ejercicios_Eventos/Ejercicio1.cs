using System;
using System.Threading;

namespace Ejercicios_Eventos
{
    public class Ejercicio1
    {
        // Clase de transacción con sus atributos
        public class Transaccion
        {
            public int identificador { get; set; }
            public string fechaTransaccion { get; set; }
        }

        // EventArgs personalizado para un evento de transacción
        public class TransaccionEventArgs : EventArgs
        {
            public Transaccion transaccion { get; set; }
        }

        // Clase para la gestión de pagos (Pasarela)
        public class PasarelaDePago
        {
            // Evento de fin de transacción 
            public event EventHandler<TransaccionEventArgs> TransaccionFinalizada;

            // Método que dispara el evento de fin de transacción
            protected virtual void EnTransaccionFinalizada(Transaccion transaccion_)
            {
                // Disparo el evento de fin de transacción, mediante una llamada al mismo, solo si es distinto de null
                if (TransaccionFinalizada != null)
                    TransaccionFinalizada(this, new TransaccionEventArgs { transaccion = transaccion_ });
            }

            // Método que procesa el pago mediante una transacción 
            public void Pago(Transaccion transaccion_)
            {
                // Procesando transacción de pago 
                Thread.Sleep(2000);
                // Pago aprobado y fin de transacción
                EnTransaccionFinalizada(transaccion_);
            }
        }
        // Definimos la clase 
        public class GestorDeEmail
        {
            // Envia la notificación por correo y le ponemos de argumentos que necesita el EventHandler
            public void EnviarNotificacion(object sender, TransaccionEventArgs e)
            {
                // Imprimos por consola
                Console.WriteLine($"Notificación: La transacción {e.transaccion.identificador} ha sido procesada con éxito.");
            }
        }
        
        // Creamos el main "pruebas"
        public static void Test()
        {
            // Crear instancia PasarelaDePago y GestorDeEmail
            PasarelaDePago pasarela = new PasarelaDePago();
            GestorDeEmail gestorEmail = new GestorDeEmail();

            // Se activa cuando el EventHandler mande la señal de evento y EnviarNotificacion del GestorDeEmail al evento TransaccionFinalizada
            pasarela.TransaccionFinalizada += gestorEmail.EnviarNotificacion;

            // Crear una transacción y procesa el pago
            Transaccion transaccion = new Transaccion { identificador = 45672341, fechaTransaccion = "2024-11-10" };
            pasarela.Pago(transaccion);
        }
    }
}
