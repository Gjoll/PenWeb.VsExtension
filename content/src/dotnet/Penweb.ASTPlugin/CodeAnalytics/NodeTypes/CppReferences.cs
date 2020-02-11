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

        [JsonProperty] public string ClassName     { get; set; }

        [JsonProperty] public string ItemName    { get; set; }

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

                
                //ICppQualifiedNamePart cppQualifiedNamePart = this.QualifiedReference.NamePart;
                //ICppExpressionNode cppExpressionNode = this.MemberAccessExpression.Qualifier;
                CppTypeAndCategory cppTypeAndCatagory = this.QualifiedReference.GetTypeAndCategory();
                CppQualType cppQualType = cppTypeAndCatagory.Type;

                CppTypeVisitor cppTypeVisitor = new CppTypeVisitor();

                cppQualType.Accept(cppTypeVisitor);

                string typeStr = cppTypeVisitor.TypeBuilder.ToString();
                string dbgStr = cppTypeVisitor.DbgBuilder.ToString().Trim();

                this.ClassName = cppTypeVisitor.NameBuilder.ToString().Trim();

                if (String.IsNullOrWhiteSpace(this.ClassName))
                {
                    this.ClassName = dbgStr;

                    Console.WriteLine($"PenWebMemberAccessExpression() class name empty");

                    cppQualType.Accept(cppTypeVisitor);

                    typeStr = cppTypeVisitor.TypeBuilder.ToString();
                    dbgStr = cppTypeVisitor.DbgBuilder.ToString();

                    this.ClassName = cppTypeVisitor.NameBuilder.ToString();
                }


                /*
                ICppExpression leftArguement = this.MemberAccessExpression.GetLeftArgument();
                CppExpressionVisitor cppExpressionVisitor = new CppExpressionVisitor();
                leftArguement.Accept<CppExpressonVisitorResult>(cppExpressionVisitor);
                */

                base.Init();

                this.CppFunctionCatagory = CppFunctionCatagory.VariableRef;

                PenWebDeclaration penWebDeclaration = this.GetParentByType<PenWebDeclaration>();

                if (penWebDeclaration == null)
                {
                }

                if (!String.IsNullOrWhiteSpace(this.ItemName) && !String.IsNullOrWhiteSpace(this.ClassName))
                {
                    this.SaveToJson = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            this.QualifiedReference = null;
        }

        public override string ToString()
        {
            return $"[{this.Location.ToString()}]  {this.GetType().Name} ClassName: {this.ClassName} ItemName: {this.ItemName}  Code: |{SingleLineText}|";
        }

    }
}
