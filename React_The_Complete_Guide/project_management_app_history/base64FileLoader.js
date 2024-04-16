import fs from 'node:fs';

export default function base64Loader() {
    return {
        name: "base64-loader",
        transform(_, id) {
            const [path, query] = id.split("?");
            if (query != "base64") {
                return null;
            }

            const data = fs.readFileSync(path);
            const base64 = data.toString("base64");

            return `export default 'data:image/png;base64,${base64}';`;
        }
    };
}