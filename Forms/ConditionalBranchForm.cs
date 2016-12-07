namespace Xceno.CustomWorkflowsDemo.Forms
{
	using System;
	using Activities;
	using Orchard.DisplayManagement;
	using Orchard.Forms.Services;
	using Orchard.Localization;
	using Orchard.Utility;
	using ViewModels;

	public class ConditionalBranchForm : IFormProvider
	{
		public ConditionalBranchForm(IShapeFactory shapeFactory)
		{
			this.Shape = shapeFactory;
			this.T = NullLocalizer.Instance;
		}

		public dynamic Shape { get; set; }

		public Localizer T { get; set; }

		public void Describe(DescribeContext context)
		{
			Func<IShapeFactory, dynamic> formFactory = shape =>
			{
				var form = this.Shape.Form(
					Id: ConditionalBranchTask.EventName,
					_Type: this.Shape.FieldSet(
						Title: this.T("Resume the workflow on a conditional branch."),
						_FirstBranch: this.Shape.Textbox(
							Id: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.FirstBranch),
							Name: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.FirstBranch),
							Title: this.T("The first branch"),
							Description: this.T("A comma separated list of something"),
							Classes: new[] { "text", "large", "tokenized" }),
						_SecondBranch: this.Shape.Textbox(
							Id: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.SecondBranch),
							Name: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.SecondBranch),
							Title: this.T("The second branch"),
							Description: this.T("A comma separated list of something"),
							Classes: new[] { "text", "large", "tokenized" }),
						_ThirdBranch: this.Shape.Checkbox(
							Id: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.ThirdBranch),
							Name: ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.ThirdBranch),
							Title: this.T("The third branch"),
							Description: this.T("Check to enable the branch"),
							Value: false)
						));

				return form;
			};

			context.Form(ConditionalBranchTask.EventName, formFactory);
		}
	}
}