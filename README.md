# OpenRename

OpenRename is a local-first Windows batch renamer. It previews every filename change, blocks collisions before execution, and supports undoing the last rename plan in the current session.

Version 1.0 executes each batch as a two-phase transaction. File swaps and case-only renames are supported, every source and destination is preflighted, and a failed operation rolls staged files back to their original names instead of leaving a half-renamed batch.

MIT licensed.
