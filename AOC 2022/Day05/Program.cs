var isSettingUpInput = true;

var finalState = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(Array.Empty<Stack<char>>(), (state, line) =>
    {
        if (state.Length == 0)
        {
            state = new Stack<char>[(int)Math.Ceiling(line.Length / 4.0)];

            for (int i = 0; i < state.Length; i++)
            {
                state[i] = new Stack<char>();
            }
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            isSettingUpInput = false;

            for (int i = 0; i < state.Length; i++)
            {
                var newStack = new Stack<char>();

                while (state[i].Count > 0)
                {
                    newStack.Push(state[i].Pop());
                }

                state[i] = newStack;
            }

            return state;
        }

        if (isSettingUpInput)
        {
            for (int i = 0; i < state.Length; i++)
            {
                // 1, 5, 9, etc.
                var crateLetter = line[(i * 4) + 1];

                if (crateLetter == ' ')
                {
                    continue;
                }

                if (crateLetter < 65)
                {
                    break;
                }

                state[i].Push(crateLetter);
            }
        }
        else
        {
            var parts = line.Split(' ');

            var oldStack = state[int.Parse(parts[3]) - 1];
            var newStack = state[int.Parse(parts[5]) - 1];

            for (int i = 0; i < int.Parse(parts[1]); i++)
            {
                newStack.Push(oldStack.Pop());
            }
        }

        return state;
    });

Console.WriteLine($"Part 1 Total: {string.Join("", finalState.Select(s => s.Peek()))}");



isSettingUpInput = true;

var finalStateV2 = (await File.ReadAllLinesAsync("Input.txt"))
    .Aggregate(Array.Empty<Stack<char>>(), (state, line) =>
    {
        if (state.Length == 0)
        {
            state = new Stack<char>[(int)Math.Ceiling(line.Length / 4.0)];

            for (int i = 0; i < state.Length; i++)
            {
                state[i] = new Stack<char>();
            }
        }

        if (string.IsNullOrWhiteSpace(line))
        {
            isSettingUpInput = false;

            for (int i = 0; i < state.Length; i++)
            {
                var newStack = new Stack<char>();

                while (state[i].Count > 0)
                {
                    newStack.Push(state[i].Pop());
                }

                state[i] = newStack;
            }

            return state;
        }

        if (isSettingUpInput)
        {
            for (int i = 0; i < state.Length; i++)
            {
                // 1, 5, 9, etc.
                var crateLetter = line[(i * 4) + 1];

                if (crateLetter == ' ')
                {
                    continue;
                }

                if (crateLetter < 65)
                {
                    break;
                }

                state[i].Push(crateLetter);
            }
        }
        else
        {
            var parts = line.Split(' ');

            var oldStack = state[int.Parse(parts[3]) - 1];
            var newStack = state[int.Parse(parts[5]) - 1];
            var cratesToMove = int.Parse(parts[1]);

            var tempStack = new Stack<char>();

            for (int i = 0; i < cratesToMove; i++)
            {
                tempStack.Push(oldStack.Pop());
            }

            for (int i = 0; i < cratesToMove; i++)
            {
                newStack.Push(tempStack.Pop());
            }
        }

        return state;
    });

Console.WriteLine($"Part 2 Total: {string.Join("", finalStateV2.Select(s => s.Peek()))}");

