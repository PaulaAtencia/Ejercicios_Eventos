using System;
using System.Threading;

namespace Ejercicios_Eventos
{
    public class Ejercicio2
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
                Console.WriteLine($"Notificación: La transacción {e.transaccion.identificador} ha sido procesada con éxito.");
            }
        }

        // Definimos la clase 
        public class GestorDeFacturacion
        {
            // Envía la factura al cliente y le ponemos los argumentos que necesita el evento "EventHandler"
            public void EmitirFactura(object sender, TransaccionEventArgs e)
            {
                Console.WriteLine($"La factura correspondiente a la transacción {e.transaccion.identificador} fue emitida con fecha {e.transaccion.fechaTransaccion}.");
            }
        }

        // Creamos el main "pruebas"
        public static void Test()
        {
            //  Creamos la instancia de: PasarelaDePago, GestorDeEmail y GestorDeFacturacion
            PasarelaDePago pasarela = new PasarelaDePago();
            GestorDeEmail gestorEmail = new GestorDeEmail();
            GestorDeFacturacion gestorFacturacion = new GestorDeFacturacion();

            // Añadimos esas dos funciones al EventHandler para que se ejecuten las dos en ese orden cuando se llame al EventHandler.
            pasarela.TransaccionFinalizada += gestorEmail.EnviarNotificacion;
            pasarela.TransaccionFinalizada += gestorFacturacion.EmitirFactura;

            // Crea una transacción y procesa el pago
            Transaccion transaccion = new Transaccion { identificador = 71645331, fechaTransaccion = "30/06/2010" };
            pasarela.Pago(transaccion);
        }
    }
}
