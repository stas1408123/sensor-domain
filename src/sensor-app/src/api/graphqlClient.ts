import { ApolloClient, InMemoryCache, HttpLink } from '@apollo/client';
import { GRAPHQL_URL } from '../constants';

const link = new HttpLink({
  uri: GRAPHQL_URL,
});

export const client = new ApolloClient({
  link,
  cache: new InMemoryCache(),
});
