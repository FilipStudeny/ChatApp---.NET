import { Routes, Route } from "react-router-dom";
import "./App.css";

import NotFound from "./routes/ErrorPages/NotFound";
import Layout from "./routes/Layout";

const App = () => (
	<Routes>
		<Route element={<Layout />}>
			<Route path='*' element={<NotFound />} />
		</Route>
	</Routes>
);

export default App;