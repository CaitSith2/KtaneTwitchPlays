﻿public sealed class CommandQueueItem
{
	public string Command { get; private set; }
	public string User { get; private set; }
	public string Name { get; private set; }
	public CommandQueueItem(string command, string user, string name = null)
	{
		Command = command;
		User = user;
		Name = name;
	}
}