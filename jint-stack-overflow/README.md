This project demonstrates an issue with function JsValue.ToObject.
It causes a StackOverflowException for objects with circular references.

| Version              | Status   |
| -------------------- | -------- |
| Jint 2.11.10         | fails    |
| Jint 3.0.0-beta-1353 | fails    |
