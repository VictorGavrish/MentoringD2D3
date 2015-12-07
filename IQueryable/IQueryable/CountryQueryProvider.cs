using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;

using CommonEntities.Entities;

using IQueryable;

using Newtonsoft.Json;

using Serialize.Linq.Extensions;
using Serialize.Linq.Serializers;

namespace IQueryable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using CommonEntities.Entities;

    using Newtonsoft.Json;

    using Serialize.Linq.Extensions;
    using Serialize.Linq.Serializers;

    class RemoveTopLevelExpressionVisitor : ExpressionVisitor
    {
        private bool topLevel = true;

        protected override Expression VisitBinary(BinaryExpression node)
        {
            this.topLevel = false;
            return base.VisitBinary(node);
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            this.topLevel = false;
            return base.VisitBlock(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            this.topLevel = false;
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            this.topLevel = false;
            return base.VisitConstant(node);
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            this.topLevel = false;
            return base.VisitDebugInfo(node);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            this.topLevel = false;
            return base.VisitDynamic(node);
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            this.topLevel = false;
            return base.VisitDefault(node);
        }

        protected override Expression VisitExtension(Expression node)
        {
            this.topLevel = false;
            return base.VisitExtension(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            this.topLevel = false;
            return base.VisitGoto(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            this.topLevel = false;
            return base.VisitInvocation(node);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            this.topLevel = false;
            return base.VisitLabelTarget(node);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            this.topLevel = false;
            return base.VisitLabel(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            this.topLevel = false;
            return base.VisitLambda(node);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            this.topLevel = false;
            return base.VisitLoop(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            this.topLevel = false;
            return base.VisitMember(node);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            this.topLevel = false;
            return base.VisitIndex(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            this.topLevel = false;
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            this.topLevel = false;
            return base.VisitNewArray(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            this.topLevel = false;
            return base.VisitNew(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            this.topLevel = false;
            return base.VisitParameter(node);
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            this.topLevel = false;
            return base.VisitRuntimeVariables(node);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            this.topLevel = false;
            return base.VisitSwitchCase(node);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            this.topLevel = false;
            return base.VisitSwitch(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            this.topLevel = false;
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            this.topLevel = false;
            return base.VisitTry(node);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            this.topLevel = false;
            return base.VisitTypeBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            bool currentTop = this.topLevel;
            this.topLevel = false;
            if (currentTop
                && (node.NodeType == ExpressionType.Convert || node.NodeType == ExpressionType.ConvertChecked))
            {
                return base.Visit(node.Operand);
            }
            return base.VisitUnary(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            this.topLevel = false;
            return base.VisitMemberInit(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            this.topLevel = false;
            return base.VisitListInit(node);
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            this.topLevel = false;
            return base.VisitElementInit(node);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            this.topLevel = false;
            return base.VisitMemberBinding(node);
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            this.topLevel = false;
            return base.VisitMemberAssignment(node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            this.topLevel = false;
            return base.VisitMemberMemberBinding(node);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            this.topLevel = false;
            return base.VisitMemberListBinding(node);
        }

    }

    public class CountryQueryProvider : IQueryProvider
    {
        private static readonly MediaTypeFormatter Formatter = new JsonMediaTypeFormatter
        {
            SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            }
        };

        private static readonly MediaTypeWithQualityHeaderValue MediaTypeJson = new MediaTypeWithQualityHeaderValue("application/json");

        public IQueryable<T> CreateQuery<T>(Expression expression)
        {
            if (!typeof(T).IsAssignableFrom(typeof(Country)))
            {
                throw new InvalidOperationException();
            }
            return (IQueryable<T>)new CountryData(this, expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public T Execute<T>(Expression expression)
        {
            using (var client = PrepareHttpClient())
            {
                var removeVisitor = new RemoveTopLevelExpressionVisitor();
                var newexpr = removeVisitor.Visit(expression);
                var expressionSerializer = new ExpressionSerializer(new Serialize.Linq.Serializers.JsonSerializer());
                var text = expressionSerializer.SerializeText(expression);
                var query = expression.ToExpressionNode();
                var response = client.PostAsync("api/country", query, Formatter, MediaTypeJson, CancellationToken.None).Result;
                return response.Content.ReadAsAsync<T>().Result;
            }
        }

        private static HttpClient PrepareHttpClient()
        {
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:9000/") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(MediaTypeJson);
            return client;
        }


        private static IEnumerable<Person> GetAllPeople()
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://localhost:9000/api/people/").Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<IEnumerable<Person>>(response);
            return result;
        }

        private static Person GetPersonById(int id)
        {
            var client = new HttpClient();
            var response = client.GetAsync($"http://localhost:9000/api/people/{id}").Result.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Person>(response);
            return result;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}