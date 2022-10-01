using Hangfire.Server;
using System;
using System.Threading.Tasks;

namespace Hangfire.API
{
    public class HangfireExamples
    {
        public void StartExamples()
        {
            // Fire-and-forget jobs: são executados apenas uma única vez e quase que imediatamente após a criação.
            BackgroundJob.Enqueue(() => FireAnfForgetExample(null));

            // Recurring jobs ou tarefas recorrentes: como eu disse no início do artigo, são tarefas que executam de tempos em tempos e são configuradas utilizando uma expressão CRON.
            RecurringJob.AddOrUpdate(() => RecurringJobExample(null), Cron.Minutely);

            // Delayed jobs ou tarefas agendadas: talvez você queira agendar uma tarefa para ser processada amanhã ou daqui uma uma semana… ou quem sabe daqui um mês? Com as tarefas agendadas você podemos processar uma tarefa quando quisermos.
            BackgroundJob.Schedule(() => DelayedJobExample(null), TimeSpan.FromDays(7)); //processe daqui uma semana

            //Continuations ou continuações: são “tarefas secundárias” cujas execuções acontecem após a "tarefa principal" ser processada.
            var jobId = BackgroundJob.Enqueue(() => ContinuationTaskFirst(null));
            BackgroundJob.ContinueJobWith(jobId, () => ContinuationTaskSecond(null));
        }

        // Fire-and-forget jobs: são executados apenas uma única vez e quase que imediatamente após a criação.
        public async Task FireAnfForgetExample(PerformContext performContext)
        {
            await Task.Run(() =>
            {
                var jobId = performContext.BackgroundJob.Id;

                // teste sucesso
                Console.WriteLine($"[FireAnfForget] Olá Hangfire! (jobId:{jobId})");

                // teste falha (Caso a tarefa falhe, por padrão o Hangfire irá retentar 10 vezes a tarefa antes de dizer que realmente falhou)
                //throw new Exception("[FireAnfForget] Oush! Uma falha ocorreu!!1");
            });
        }

        // Recurring jobs ou tarefas recorrentes: como eu disse no início do artigo, são tarefas que executam de tempos em tempos e são configuradas utilizando uma expressão CRON.
        public async Task RecurringJobExample(PerformContext performContext)
        {
            await Task.Run(() =>
            {
                var jobId = performContext.BackgroundJob.Id;

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"[RecurringJob] {0} (jobId:{jobId})", i);
                }
            });
        }

        // Delayed jobs ou tarefas agendadas: talvez você queira agendar uma tarefa para ser processada amanhã ou daqui uma uma semana… ou quem sabe daqui um mês? Com as tarefas agendadas você podemos processar uma tarefa quando quisermos.
        public async Task DelayedJobExample(PerformContext performContext)
        {
            await Task.Run(() =>
            {
                var jobId = performContext.BackgroundJob.Id;

                Console.WriteLine($"[DelayedJobExample] Olá Hangfire (agendado)! (jobId:{jobId})");
            });
        }

        //Continuations ou continuações: são “tarefas secundárias” cujas execuções acontecem após a "tarefa principal" ser processada.
        public async Task ContinuationTaskFirst(PerformContext performContext)
        {
            await Task.Run(() =>
            {
                var jobId = performContext.BackgroundJob.Id;

                Console.WriteLine($"[ContinuationTaskFirst] Tarefa principal executada! (jobId:{jobId})");
            });
        }

        //Continuations ou continuações: são “tarefas secundárias” cujas execuções acontecem após a "tarefa principal" ser processada.
        public async Task ContinuationTaskSecond(PerformContext performContext)
        {
            await Task.Run(() =>
            {
                var jobId = performContext.BackgroundJob.Id;

                Console.WriteLine($"[ContinuationTaskSecond] Tarefa secundária executada! (jobId:{jobId})");
            });
        }
    }
}
