namespace Xceno.CustomWorkflowsDemo.Activities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Orchard.Localization;
	using Orchard.Utility;
	using Orchard.Workflows.Models;
	using Orchard.Workflows.Services;
	using ViewModels;

	public class ConditionalBranchTask : Task
	{
		public const string EventName = "ConditionalBranch";
		public const string FirstBranchOutcome = "FirstBranchDone";
		public const string ThirdBranchOutcome = "SecondBranchDone";
		public const string ErrorOutcome = "Error";
		public const string DoneOutcome = "Done";

		public ConditionalBranchTask()
		{
			this.T = NullLocalizer.Instance;
		}

		public Localizer T { get; set; }

		public override string Name
		{
			get { return EventName; }
		}

		public override string Form
		{
			get { return EventName; }
		}

		public override LocalizedString Category
		{
			get { return this.T("Flow"); }
		}

		public override LocalizedString Description
		{
			get { return this.T("Branches the workflow based on what fields are provided."); }
		}

		public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext)
		{
			return true;
		}

		public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
		{
			var possibleOutcomes = GetBranches(activityContext);

			return possibleOutcomes.Select(x => this.T(x));
		}

		public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
		{
			// Do whatever here....
			// This stuff here is obviously pretty useless and only exists to see if the activity works.
			var possibleOutcomes = GetBranches(activityContext);

			var firstBranchValue = activityContext.GetState<string>(ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.FirstBranch));
			if ( possibleOutcomes.Contains(FirstBranchOutcome) && !string.IsNullOrWhiteSpace(firstBranchValue) && firstBranchValue.Equals("ThrowError", StringComparison.OrdinalIgnoreCase) )
			{
				yield return this.T(ErrorOutcome);
			}
			else
			{
				foreach ( var outcome in possibleOutcomes )
				{
					yield return this.T(outcome);
				}
			}
		}

		// Ripped from the original BranchActivity and modified
		private static IEnumerable<string> GetBranches(ActivityContext context)
		{
			var branches = new List<string>();

			var firstBranch = context.GetState<string>(ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.FirstBranch));
			if ( firstBranch != null )
			{
				branches.Add(FirstBranchOutcome);
			}

			var secondBranch = context.GetState<string>(ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.SecondBranch));
			if ( !string.IsNullOrEmpty(secondBranch) )
			{
				branches.AddRange(secondBranch.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList());
			}

			var thirdBranch = context.GetState<bool>(ReflectOn<ConditionalBranchViewModel>.NameOf(x => x.ThirdBranch));
			if ( thirdBranch )
			{
				branches.Add(ThirdBranchOutcome);
			}

			branches.AddRange(new[] { ErrorOutcome, DoneOutcome });

			return branches;
		}
	}
}