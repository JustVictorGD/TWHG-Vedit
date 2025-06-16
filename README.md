Folder explanations:

- Common: Widely usable static functions for shortening common math.

- Objects: Things you'd expect to visibly make up scenes.

- Engine: Features that give off engine vibes more than game ones.

- Types: Structs which are vital for managing spacial data.



Project design guide:

- Use tabs for indents.

- The base levels of .cs files should be styled with these groups, separated by single empty lines:

    - Namespace keyword.

    - File description (optional). It's allowed to include additional empty lines for clarity if it's long.

    - Using external namespaces such as System, Math and Raylib-cs (optional).

    - Using internal namespaces (optional). Exclude the "WhgVedit." start.

    - Class/struct keyword.

- If a feature is unstable or unfinished, make it clear by adding a descriptive description.

- Shorten all instances of "Rectangle" to "Rect" in properties and methods.



Default ZIndex categories:

- Floor: -1000

- Checkpoint: -800

- Conveyor: -600

- Ice: -400

- Water: -200

- Coin: 200

- Key: 250

- Paint: 300

- Enemy: 400

- Player: 600

- Door: 800

- Golden Door: 900

- Wall: 1000
