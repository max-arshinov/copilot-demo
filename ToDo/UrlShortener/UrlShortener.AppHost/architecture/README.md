## 🔗 URL Shortener

### 📋 Requirements

- **1 million new users per year**, each shortening **100 links per year on average**.
- Each shortened link must be stored for **3 years**.
- **Redirect latency** must not exceed **0.2 seconds**.
- **Click statistics must be tracked**.
- System **releases must be seamless**, with no downtime.
- Average **link traffic**: 10 clicks per day per link.
- _**Failure of any two nodes must not degrade system performance by more than 0.1s.**_

---

### ✅ Task

- [ ] Perform back-of-the-napkin calculations.
- [ ] Choose and justify a data storage model, including:
    - Storage and cleanup of expired links
    - Storage and update of click statistics
    - Any additional required functional components
- [ ] _**Choose and justify server infrastructure, required components, technologies, and libraries.**_
- [ ] Justify the system's **reliability and fault tolerance**.
- [ ] Select and justify a **release strategy** and corresponding tooling.
- [ ] Define and justify **team composition and skill requirements**.
- [ ] Select and justify **development environments and tooling**.
- [ ] Choose and justify a **hosting provider**; estimate **server costs** across all environments including production.
