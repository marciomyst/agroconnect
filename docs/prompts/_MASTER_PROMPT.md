You are working on the Agronomia backend (.NET 10).

This repository follows:
- DDD
- Clean Architecture
- CQRS with Wolverine
- Dapper for read-only repositories
- Explicit Application Services
- Vertical Slice per feature

You MUST follow:
- UBIQUOUS_LANGUAGE.md
- ARCHITECTURE.md
- AGENTS.md
- DOMAIN_EVENTS.md

Rules:
- Do NOT invent new domain concepts
- Do NOT rename existing concepts
- Do NOT refactor existing code unless explicitly asked
- Aggregates must not reference other aggregates
- All writes go through Application Services
- Domain Events must be explicit

If something is unclear, STOP and ask.
