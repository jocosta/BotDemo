using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using BotNetSpDemo.WebCrawler;
using System.Threading.Tasks;

namespace BotNetSpDemo
{
    public enum Sandwiches
    {

    }

    public enum Drinks
    {

    }

    public enum PaymentType
    {

    }

    [Serializable]
    public class PedidoForm
    {
        public string Name { get; set; }
        public string Sandwiche { get; set; }
        public Drinks Drink { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsVoyage { get; set; }

        public static IForm<PedidoForm> BuildForm()
        {

            var builder = new FormBuilder<PedidoForm>()
                .Message("Bem-vindo ao bot do MC! \U0001F609")
                .Field(new FieldReflector<PedidoForm>(nameof(Sandwiche))
                            .SetType(null)
                            .SetDefine((state, field) =>
                            {
                                var crawler = new McDonaldsBrSite();
                                var dados = crawler.GetMeatMenu().Distinct();

                                foreach (var prod in dados)
                                    field
                                        .SetAllowsMultiple(true)
                                        .SetPrompt(new PromptAttribute("Selecione o seu sanduiche {||}"))
                                        .AddDescription(prod, prod.SandwicheName, prod.ImgUrl)
                                        .AddTerms(prod, prod.SandwicheName);

                                return Task.FromResult(true);
                            }))
                 .AddRemainingFields();
              

            builder.Confirm("Legal, finalizamos o preenchimento do seu cadastro. Pra finalizar, confirme os dados abaixo: {*} {||}");
            //builder.OnCompletion(async (context, cadastro) =>
            //{



            //                 //activity.Text = $"{cadastro.NomeCompleto}, seu cadastro foi realizado com sucesso. Em instantes chegará no email  {cadastro.Email} um novo e-mail, fique ligado!";
            //    await context.PostAsync(null);
            //});   //IMessageActivity activity = context.MakeMessage();


            var form = builder.Build();


            return form;
        }


    }
}