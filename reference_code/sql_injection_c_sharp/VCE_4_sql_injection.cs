Query HQLQuery = session.createQuery("FROM accounts WHERE custID='" + request.getParameter("id") + "'");