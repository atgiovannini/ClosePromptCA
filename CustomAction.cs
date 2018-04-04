using System;
using Microsoft.Deployment.WindowsInstaller;

namespace ClosePromptCA
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult ClosePrompt(Session session)
        {
            Logger.session = session;
            Logger.Write("Begin PromptToCloseApplications");
            try
            {
                var productName = session["ProductName"];
                Logger.Write("Current Product are {0}", productName);

                var processes = session["PromptToCloseProcesses"].Split(',');
                var displayNames = session["PromptToCloseDisplayNames"].Split(',');

                if (processes.Length != displayNames.Length)
                {
                    Logger.Write(@"Please check that 'PromptToCloseProcesses' and 'PromptToCloseDisplayNames' exist and have same number of items.");
                    return ActionResult.Failure;
                }

                for (var i = 0; i < processes.Length; i++)
                {
                    if (String.IsNullOrEmpty(processes[i]))
                    {
                        Logger.Write("Found empty value at index {0}", i);
                        continue;
                    }
                    Logger.Write("Prompting process {0} with name {1} to close.", processes[i], displayNames[i]);
                    using (var prompt = new PromptCloseApplication(productName, processes[i], displayNames[i]))
                        if (!prompt.Prompt(session))
                            return ActionResult.Failure;
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Missing properties or wrong values. Please check that 'PromptToCloseProcesses' and 'PromptToCloseDisplayNames' exist and have same number of items. \nException:" + ex.Message);
                return ActionResult.Failure;
            }

            Logger.Write("End PromptToCloseApplications");
            return ActionResult.Success;
        }


        [CustomAction]
        public static ActionResult CloseHidden(Session session)
        {
            Logger.Write("Begin ToCloseApplicationsHidden");
            try
            {
                var productName = session["ProductName"];
                Logger.Write("Current Product are {0}", productName);

                var processes = session["PromptToCloseProcesses"].Split(',');
                var displayNames = session["PromptToCloseDisplayNames"].Split(',');

                if (processes.Length != displayNames.Length)
                {
                    Logger.Write(@"Please check that 'PromptToCloseProcesses' and 'PromptToCloseDisplayNames' exist and have same number of items.");
                    return ActionResult.Failure;
                }

                for (var i = 0; i < processes.Length; i++)
                {
                    if (String.IsNullOrEmpty(processes[i]))
                    {
                        Logger.Write("Found empty value at index {0}", i);
                        continue;
                    }
                    Logger.Write("Killing process {0} with name {1}", processes[i], displayNames[i]);
                    using (var prompt = new PromptCloseApplication(productName, processes[i], displayNames[i]))
                        if (!prompt.Close(session))
                        {
                            Logger.Write("Process kill failed with no args");
                            return ActionResult.Failure;
                        }
                }
            }
            catch (Exception ex)
            {
                Logger.Write("Missing properties or wrong values. Please check that 'PromptToCloseProcesses' and 'PromptToCloseDisplayNames' exist and have same number of items. \nException:" + ex.Message);
                return ActionResult.Failure;
            }

            Logger.Write("End ToCloseApplicationsHidden");
            return ActionResult.Success;
        }

    }
}
