using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.FormFlow.Advanced;
using BotNetSpDemo.WebCrawler;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace BotNetSpDemo
{
    public enum PaymentType
    {
        Credito,
        Debito,
        ValeRefeicao,
        Dinheiro
    }

    [Serializable]
    public class PedidoForm
    {
        [Prompt("Qual o seu nome?")]
        public string Name { get; set; }
        public string Sandwiche { get; set; }
        public string Garnish { get; set; }
        public string Drink { get; set; }
        [Prompt("Batata grande por mais R$1,00? {||}")]
        public bool AddShipsFor1Real { get; set; }
        [Prompt("Qual a forma de pagamento: {||}")]
        public PaymentType PaymentType { get; set; }
        [Prompt("Pedido para viagem? {||}")]
        public bool IsVoyage { get; set; }

        public static IForm<PedidoForm> BuildForm()
        {

            var builder = new FormBuilder<PedidoForm>()
                .Message("Bem-vindo ao bot do MC! \U0001F609")
                .Field(nameof(Name))
                .Field(new FieldReflector<PedidoForm>(nameof(Sandwiche))
                .SetType(null)
                .SetActive((state) =>
                {
                    return string.IsNullOrEmpty(state.Sandwiche);
                })
                .SetPrompt(new PromptAttribute("Por favor, selecione o sanduíche: {||}")
                {
                    ChoiceStyle = ChoiceStyleOptions.Carousel

                })
                .SetDefine((state, field) =>
                {
                    var crawler = new McDonaldsBrSite();
                    var result = crawler.GetMenu().Distinct();

                    foreach (var item in result)
                    {
                        field
                        .AddDescription(item.ItemName, item.ItemName, item.ImgUrl)
                        .AddTerms(item.ItemName, item.ItemName);
                    }

                    return Task.FromResult(true);
                }))
                .Field(new FieldReflector<PedidoForm>(nameof(Garnish))
                .SetType(null)
                .SetActive((state) =>
                {
                    return string.IsNullOrEmpty(state.Garnish);
                })
                .SetPrompt(new PromptAttribute("Por favor, selecione o acompanhamento: {||}")
                {
                    ChoiceStyle = ChoiceStyleOptions.Carousel

                })
                .SetDefine((state, field) =>
                {
                    var crawler = new McDonaldsBrSite();
                    var result = crawler.GetGarnish().Distinct();

                    foreach (var item in result)
                    {
                        field
                        .AddDescription(item.ItemName, item.ItemName, item.ImgUrl)
                        .AddTerms(item.ItemName, item.ItemName);
                    }

                    return Task.FromResult(true);
                }))
                .Field(new FieldReflector<PedidoForm>(nameof(Drink))
                .SetType(null)
                .SetActive((state) =>
                {
                    return string.IsNullOrEmpty(state.Drink);
                })
                .SetPrompt(new PromptAttribute("Selecione a sua bebida {||}")
                {
                    ChoiceStyle = ChoiceStyleOptions.Carousel

                })
                .SetDefine((state, field) =>
                {
                    var crawler = new McDonaldsBrSite();
                    var result = crawler.GetDrink().Distinct();

                    foreach (var item in result)
                    {
                        field
                        .AddDescription(item.ItemName, item.ItemName, item.ImgUrl)
                        .AddTerms(item.ItemName, item.ItemName);
                    }

                    return Task.FromResult(true);
                }))
                .AddRemainingFields();
                                         


            builder.Confirm("Legal, finalizamos o preenchimento do seu pedido. Pra finalizar, você confirma os dados abaixo? {*} {||}");
            builder.OnCompletion(async (context, pedido) =>
            {
                TrelloHelper.PublishIntoTrello(pedido.Name,
                                               pedido.Sandwiche,
                                               pedido.Drink,
                                               pedido.Garnish,
                                               pedido.AddShipsFor1Real.ToString(),
                                               pedido.IsVoyage.ToString(),
                                               pedido.PaymentType.ToString());

                var userAccount = new ChannelAccount(name: context.Activity.From.Name, id: context.Activity.From.Id);
                var botAccount = new ChannelAccount(name: context.Activity.Recipient.Name, id: context.Activity.Recipient.Id);
                var connector = new ConnectorClient(new Uri(context.Activity.ServiceUrl));
                var conversationId = await connector.Conversations.CreateDirectConversationAsync(botAccount, userAccount);


                
                IMessageActivity message = Activity.CreateMessageActivity();
                message.From = botAccount;
                message.Recipient = userAccount;
                message.Conversation = new ConversationAccount(id: conversationId.Id);
                message.Text = $"{pedido.Name}, seu pedido foi realizado com sucesso. Assim que estiver pronto te aviso, fique ligado!";
                message.Locale = "pt-BR";
                await context.PostAsync(message);

            });   //IMessageActivity activity = context.MakeMessage();


            var form = builder.Build();


            return form;
        }


    }
}