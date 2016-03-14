namespace XML
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class YellowBookExpressionVisitor : ExpressionVisitor
    {
        private string xpath = "/directory//person";

        private readonly Dictionary<string, string> fieldMap = new Dictionary<string, string>
        {
            { "FirstName", "first-name" },
            { "LastName", "last-name" },
            { "Address", "address" },
            { "Age", "age" }
        };

        private static readonly Expression<Func<Person, bool>> NopLambda = p => true;

        private readonly string filePath;

        public YellowBookExpressionVisitor(string filePath)
        {
            this.filePath = filePath;
        }

        public object Query(Expression expression)
        {
            var unparsed = this.Visit(expression);

            var elements = XDocument.Load(this.filePath).XPathSelectElements(this.xpath);

            var people = elements.Select(XElementToPerson).ToList();

            var queryable = people.AsQueryable().Where(NopLambda);

            var result = queryable.Provider.Execute(unparsed);

            return result;
        }

        private static Person XElementToPerson(XElement result)
        {
            var person = new Person
            {
                FirstName = result.XPathSelectElement("first-name").Value, 
                LastName = result.XPathSelectElement("last-name").Value, 
                Address = result.XPathSelectElement("address").Value, 
                Age = int.Parse(result.XPathSelectElement("age").Value)
            };

            return person;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " = ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.NotEqual:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " != ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.GreaterThan:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " > ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " >= ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.LessThan:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " < ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.LessThanOrEqual:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " <= ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " + ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " and ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.Divide:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " div ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " * ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " or ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    this.xpath += "[";
                    this.Visit(node.Left);
                    this.xpath += " - ";
                    this.Visit(node.Right);
                    this.xpath += "]";
                    break;
                default:
                    throw new QueryParseException($"Unsupported operation: {node.NodeType}");
            }

            return node;
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            return base.VisitBlock(node);
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            return base.VisitConditional(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Type == typeof(YellowBookQuery))
            {
                return base.VisitConstant(node);
            }

            if (IsNumeric(node.Value.GetType()))
            {
                this.xpath += node.Value;
            }
            else
            {
                this.xpath += $"'{node.Value}'";
            }

            return base.VisitConstant(node);
        }

        private static bool IsNumeric(Type type)
            =>
                type == typeof(sbyte) || type == typeof(byte) || type == typeof(short) || type == typeof(int)
                || type == typeof(long) || type == typeof(ushort) || type == typeof(uint) || type == typeof(ulong)
                || type == typeof(float) || type == typeof(double) || type == typeof(decimal);

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            return base.VisitDebugInfo(node);
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            return base.VisitDynamic(node);
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            return base.VisitDefault(node);
        }

        protected override Expression VisitExtension(Expression node)
        {
            return base.VisitExtension(node);
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            return base.VisitGoto(node);
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            return base.VisitInvocation(node);
        }

        protected override LabelTarget VisitLabelTarget(LabelTarget node)
        {
            return base.VisitLabelTarget(node);
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            return base.VisitLabel(node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            return base.VisitLambda(node);
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            return base.VisitLoop(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            this.xpath += this.fieldMap[node.Member.Name];

            return base.VisitMember(node);
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            return base.VisitIndex(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var currentXpath = this.xpath;

            try
            {
                switch (node.Method.Name)
                {
                    case "Where":
                        this.Visit(node.Arguments);
                        this.xpath = $"({this.xpath})";

                        //return Expression.Constant()

                        return node.Update(node.Object, new[] { node.Arguments[0], NopLambda });
                    case "First":
                    case "FirstOrDefault":
                        if (node.Arguments.Count > 1)
                        {
                            this.Visit(node.Arguments[1]);
                            this.xpath = $"(({this.xpath})[1])";
                        }
                        else
                        {
                            this.xpath = $"(({this.xpath})[1])";
                        }

                        return node.Update(node.Object, new Expression[] { });
                    default:
                        if (node.Arguments?[0] is MethodCallExpression)
                        {
                            var inner = (MethodCallExpression)this.Visit(node.Arguments[0]);

                            var modifiedNode = node.Update(node.Object, new[] { inner }.Concat(node.Arguments.Skip(1)));

                            return modifiedNode;
                        }

                        return base.VisitMethodCall(node);
                }
            }
            catch (QueryParseException ex)
            {
                Console.WriteLine(ex.Message);
                this.xpath = currentXpath;
                return node;
            }
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            return base.VisitNewArray(node);
        }

        protected override Expression VisitNew(NewExpression node)
        {
            return base.VisitNew(node);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return base.VisitParameter(node);
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            return base.VisitRuntimeVariables(node);
        }

        protected override SwitchCase VisitSwitchCase(SwitchCase node)
        {
            return base.VisitSwitchCase(node);
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            return base.VisitSwitch(node);
        }

        protected override CatchBlock VisitCatchBlock(CatchBlock node)
        {
            return base.VisitCatchBlock(node);
        }

        protected override Expression VisitTry(TryExpression node)
        {
            return base.VisitTry(node);
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            return base.VisitTypeBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            return base.VisitUnary(node);
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            return base.VisitMemberInit(node);
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            return base.VisitListInit(node);
        }

        protected override ElementInit VisitElementInit(ElementInit node)
        {
            return base.VisitElementInit(node);
        }

        protected override MemberBinding VisitMemberBinding(MemberBinding node)
        {
            return base.VisitMemberBinding(node);
        }

        protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
        {
            return base.VisitMemberAssignment(node);
        }

        protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
        {
            return base.VisitMemberMemberBinding(node);
        }

        protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
        {
            return base.VisitMemberListBinding(node);
        }
    }
}