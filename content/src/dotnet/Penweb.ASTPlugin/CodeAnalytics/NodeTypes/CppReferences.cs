﻿using JetBrains.DocumentModel;
using JetBrains.ReSharper.Psi.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.ReSharper.Psi.Cpp.Symbols;
using JetBrains.ReSharper.Psi.Cpp.Tree;
using Newtonsoft.Json;
using PenWeb.ASTPlugin;
using JetBrains.ReSharper.Psi.Cpp.Expressions;
using JetBrains.ReSharper.Psi.Cpp.Types;
using JetBrains.ReSharper.Psi.Cpp.Resolve;

namespace Penweb.CodeAnalytics
{
    public class PenWebQualifiedBaseTypeReference : CppParseTreeNodeBase
    {
        public PenWebQualifiedBaseTypeReference( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.QualifiedBaseTypeReference treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebQualifiedNamespaceReference : CppParseTreeNodeBase
    {
        public PenWebQualifiedNamespaceReference( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.QualifiedNamespaceReference treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebQualifiedUsingDeclarationTargetReference : CppParseTreeNodeBase
    {
        public PenWebQualifiedUsingDeclarationTargetReference( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.QualifiedUsingDeclarationTargetReference treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebOperatorFunctionId : CppParseTreeNodeBase
    {
        public PenWebOperatorFunctionId( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.OperatorFunctionId treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebParametersAndQualifiers : CppParseTreeNodeBase
    {
        public PenWebParametersAndQualifiers( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.ParametersAndQualifiers treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebNameQualifier : CppParseTreeNodeBase
    {
        public PenWebNameQualifier( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.NameQualifier treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebMSDeclSpec : CppParseTreeNodeBase
    {
        public PenWebMSDeclSpec( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.MSDeclSpec treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebFunctionArgumentList : CppParseTreeNodeBase
    {
        public PenWebFunctionArgumentList( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.FunctionArgumentList treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebFwdClassSpecifier : CppParseTreeNodeBase
    {
        public PenWebFwdClassSpecifier( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.FwdClassSpecifier treeNode ) : base(parentNode, treeNode)
        {
        }
    }

    public class PenWebQualifiedReference : CppParseTreeNodeBase
    {
        public JetBrains.ReSharper.Psi.Cpp.Tree.QualifiedReference QualifiedReference { get; set; }


        [JsonProperty] public string OwningClass { get; set; } = "";

        [JsonProperty] public string TypeName    { get; set; } = "";

        [JsonProperty] public string ItemName    { get; set; } = "";

        public PenWebQualifiedReference( CppParseTreeNodeBase parentNode, JetBrains.ReSharper.Psi.Cpp.Tree.QualifiedReference treeNode ) : base(parentNode, treeNode)
        {
            this.QualifiedReference = treeNode;
        }

        public override void Init()
        {
            try
            {
                //QualifiedReference qualifiedReference = this.QualifiedReference;

                CppQualifiedName cppQualifiedName = this.QualifiedReference.GetQualifiedName();

                this.ItemName = cppQualifiedName.GetNameStr();


                CppResolveResult cppResolveResult = this.QualifiedReference.GetResolvedReference();

                switch ( cppResolveResult.GetStatus() )
                {
                    case CppResolveResult.Status.OK:
                        ICppResolveEntity cppResolveEntity = cppResolveResult.GetPrimaryEntity();
                        if (cppResolveEntity != null)
                        {
                            /*
                            IGenericSymbolNode genericSymbolNode = cppResolveEntity.TryGetDeclarator();

                            if (genericSymbolNode != null)
                            {
                                ICppSymbol cppSymbol = genericSymbolNode.GetGenericSymbol();

                                string symbolType = cppSymbol.GetType().Name;

                                switch (cppSymbol)
                                {
                                    case CppDeclaratorSymbol cppDeclaratorSymbol:
                                        break;

                                    default:
                                        break;
                                }
                            }
                            */

                            ICppQualifiedNamePart cppQualifiedNamePart = cppResolveEntity.Name;

                            ICppResolveEntity  parentResolveEntity = cppResolveEntity.StructuralParent;

                            if (parentResolveEntity != null && parentResolveEntity.Name != null)
                            {
                                this.OwningClass = parentResolveEntity.Name.GetNameStr();
                            }

                            //string namePartStr = cppQualifiedNamePart.GetNameStr();
                        }
                        break;

                    default:
                        break;
                }

                //ICppQualifiedNamePart cppQualifiedNamePart = this.QualifiedReference.NamePart;
                //ICppExpressionNode cppExpressionNode = this.MemberAccessExpression.Qualifier;
                CppTypeAndCategory cppTypeAndCatagory = this.QualifiedReference.GetTypeAndCategory();

                CppQualType cppQualType = cppTypeAndCatagory.Type;

                CppTypeVisitor cppTypeVisitor = new CppTypeVisitor();

                cppQualType.Accept(cppTypeVisitor);

                string typeStr = cppTypeVisitor.TypeStr;
                string dbgStr = cppTypeVisitor.DbgStr;

                this.TypeName = cppTypeVisitor.DbgStr;

                /*
                ICppExpression leftArguement = this.MemberAccessExpression.GetLeftArgument();
                CppExpressionVisitor cppExpressionVisitor = new CppExpressionVisitor();
                leftArguement.Accept<CppExpressonVisitorResult>(cppExpressionVisitor);
                */

                base.Init();

                this.CppFunctionCatagory = CppFunctionCatagory.VariableRef;

                /*
                PenWebDeclaration penWebDeclaration = this.GetParentByType<PenWebDeclaration>();

                if (penWebDeclaration != null)
                {
                    string owningClass = penWebDeclaration.OwningClass;
                    string typeName = penWebDeclaration.TypeName;
                    string itemName = penWebDeclaration.VariableName;

                }
                */

                if (!String.IsNullOrWhiteSpace(this.ItemName) && !String.IsNullOrWhiteSpace(this.TypeName) && !String.IsNullOrWhiteSpace(this.OwningClass))
                {
                    this.SaveToJson = true;
                }
            }
            catch (Exception e)
            {
                LogManager.Self.Log("PenWebQualifiedReference Exception", e);
            }

            this.QualifiedReference = null;
        }

        public override string ToString()
        {
            return $"[{this.Location.ToString()}]  {this.GetType().Name} TypeName: {this.TypeName} ItemName: {this.ItemName}  Code: |{SingleLineText}|";
        }

    }
}
