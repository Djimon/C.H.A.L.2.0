# Documentation Standards

## Goal
No ellipses, no open TODOs in release docs.

## Unified Header Block (each file)
- Purpose
- Public API (signatures/events)
- Usage (example)
- Known Pitfalls
- Related (links/files)

## TODO Format (regex-friendly)
**Single line:**
```text
// TODO (<author>,<YYYY-MM-DD>): short text
```
**Multi-line**
```text
/* TODO
author: <Name>
date: <YYYY-MM-DD>
- bullet points
*/
```

## Writing Rules
- Avoid passive voice for instructions.
- Use tables for schemas.
- Use Mermaid for flows.
- Use code blocks for pseudocode and examples; never leave "...".
